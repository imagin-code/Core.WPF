﻿using ConsoleControlAPI;
using Imagin.Core.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Core.Controls;

/// <summary>
/// Interaction logic for ConsoleControl.xaml
/// </summary>
public partial class ConsoleBox : RichTextBox, IExplorer
{
    /// <summary>Occurs when console output is produced.</summary>
    public event ProcessEventHandler OnProcessOutput;

    /// <summary>Occurs when console input is produced.</summary>
    public event ProcessEventHandler OnProcessInput;

    ///

    /// <summary>Current position that input starts at.</summary>
    int inputStartPos;

    /// <summary>The last input string (used so that we can make sure we don't echo input twice).</summary>
    string lastInput;

    static readonly DependencyProperty ShowDiagnosticsProperty = DependencyProperty.Register("ShowDiagnostics", typeof(bool), typeof(ConsoleBox), new PropertyMetadata(false));
    /// <summary>Gets or sets a value indicating whether to show diagnostics.</summary>
    /// <value><c>true</c> if show diagnostics; otherwise, <c>false</c>.</value>
    public bool ShowDiagnostics
    {
        get => (bool)GetValue(ShowDiagnosticsProperty);
        set => SetValue(ShowDiagnosticsProperty, value);
    }

    private static readonly DependencyProperty IsInputEnabledProperty = DependencyProperty.Register("IsInputEnabled", typeof(bool), typeof(ConsoleBox), new PropertyMetadata(true));
    /// <summary>Gets or sets a value indicating whether this instance has input enabled.</summary>
    /// <value><c>true</c> if this instance has input enabled; otherwise, <c>false</c>.</value>
    public bool IsInputEnabled
    {
        get { return (bool)GetValue(IsInputEnabledProperty); }
        set { SetValue(IsInputEnabledProperty, value); }
    }

    internal static readonly DependencyPropertyKey IsProcessRunningPropertyKey = DependencyProperty.RegisterReadOnly("IsProcessRunning", typeof(bool), typeof(ConsoleBox), new PropertyMetadata(false));
    static readonly DependencyProperty IsProcessRunningProperty = IsProcessRunningPropertyKey.DependencyProperty;
    /// <summary>Gets a value indicating whether this instance has a process running.</summary>
    /// <value><c>true</c> if this instance has a process running; otherwise, <c>false</c>.</value>
    public bool IsProcessRunning
    {
        get { return (bool)GetValue(IsProcessRunningProperty); }
        private set { SetValue(IsProcessRunningPropertyKey, value); }
    }

    /// <summary>The internal process interface used to interface with the process.</summary>
    readonly ProcessInterface processInterface = new ProcessInterface();
    /// <summary>Gets the internally used process interface.</summary>
    /// <value>The process interface.</value>
    public ProcessInterface ProcessInterface => processInterface;

    public string Path
    {
        get => XExplorer.GetPath(this);
        set => XExplorer.SetPath(this, value);
    }

    ///

    /// <summary>
    /// Initializes a new instance of the <see cref="ConsoleBox"/> class.
    /// </summary>
    public ConsoleBox()
    {
        InitializeComponent();
            
        //  Handle process events.
        processInterface.OnProcessOutput += ProcessInterface_OnProcessOutput;
        processInterface.OnProcessError += ProcessInterface_OnProcessError;
        processInterface.OnProcessInput += ProcessInterface_OnProcessInput;
        processInterface.OnProcessExit += ProcessInterface_OnProcessExit;

        //  Wait for key down messages on the rich text box.
        PreviewKeyDown += richTextBoxConsole_PreviewKeyDown;
        StartProcess("cmd.exe", Path);
    }

    ///

    /// <summary>
    /// Handles the OnProcessError event of the processInterace control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
    void ProcessInterface_OnProcessError(object sender, ProcessEventArgs args)
    {
        //  Write the output, in red
        WriteOutput(args.Content, System.Windows.Media.Colors.Red);

        //  Fire the output event.
        FireProcessOutputEvent(args);
    }

    /// <summary>
    /// Handles the OnProcessOutput event of the processInterace control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
    void ProcessInterface_OnProcessOutput(object sender, ProcessEventArgs args)
    {
        //  Write the output, in white
        WriteOutput(args.Content, System.Windows.Media.Colors.White);

        //  Fire the output event.
        FireProcessOutputEvent(args);
    }

    /// <summary>
    /// Handles the OnProcessInput event of the processInterace control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
    void ProcessInterface_OnProcessInput(object sender, ProcessEventArgs args)
    {
        FireProcessInputEvent(args);
    }

    /// <summary>
    /// Handles the OnProcessExit event of the processInterace control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
    void ProcessInterface_OnProcessExit(object sender, ProcessEventArgs args)
    {
        //  Read only again.
        RunOnUIDispatcher(() =>
        {
            //  Are we showing diagnostics?
            if (ShowDiagnostics)
            {
                WriteOutput(Environment.NewLine + processInterface.ProcessFileName + " exited.", Color.FromArgb(255, 0, 255, 0));
            }

            IsReadOnly = true;

            //  And we're no longer running.
            IsProcessRunning = false;
        });
    }

    /// <summary>
    /// Handles the KeyDown event of the richTextBoxConsole control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.Windows.Input.KeyEventArgs" /> instance containing the event data.</param>
    void richTextBoxConsole_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        var caretPosition = this.GetCaretPosition();
        var delta = caretPosition - inputStartPos;
        var inReadOnlyZone = delta < 0;

        //  If we're at the input point and it's backspace, bail.
        if (inReadOnlyZone && e.Key == Key.Back)
            e.Handled = true;

        //  Are we in the read-only zone?
        if (inReadOnlyZone)
        {
            //  Allow arrows and Ctrl-C.
            if (!(e.Key == Key.Left ||
                e.Key == Key.Right ||
                e.Key == Key.Up ||
                e.Key == Key.Down ||
                (e.Key == Key.C && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))))
            {
                e.Handled = true;
            }
        }

        //  Is it the return key?
        if (e.Key == Key.Return)
        {
            //  Get the input.
            var input = new TextRange(this.GetPointerAt(inputStartPos), Selection.Start).Text;

            //  Write the input (without echoing).
            WriteInput(input, Foreground.As<SolidColorBrush>()?.Color ?? System.Windows.Media.Colors.White, false);
        }
    }

    /// <summary>
    /// Writes the output to the console control.
    /// </summary>
    /// <param name="output">The output.</param>
    /// <param name="color">The color.</param>
    public void WriteOutput(string output, Color color)
    {
        if (string.IsNullOrEmpty(lastInput) == false &&
            (output == lastInput || output.Replace("\r\n", "") == lastInput))
            return;

        RunOnUIDispatcher(() =>
        {
            //  Write the output.
            var range = new TextRange(this.GetEndPointer(), this.GetEndPointer())
            {
                Text = output
            };
            range.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
                
            //  Record the new input start.
            ScrollToEnd();
            this.SetCaretToEnd();
            inputStartPos = this.GetCaretPosition();
        });
    }

    /// <summary>
    /// Clears the output.
    /// </summary>
    public void ClearOutput()
    {
        Document.Blocks.Clear();
        inputStartPos = 0;
    }

    /// <summary>
    /// Writes the input to the console control.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <param name="color">The color.</param>
    /// <param name="echo">if set to <c>true</c> echo the input.</param>
    public void WriteInput(string input, Color color, bool echo)
    {
        RunOnUIDispatcher(() =>
        {
            //  Are we echoing?
            if (echo)
            {
                Selection.ApplyPropertyValue(TextBlock.ForegroundProperty, new SolidColorBrush(color));
                AppendText(input);
                inputStartPos = this.GetEndPosition();
            }

            lastInput = input;

            //  Write the input.
            processInterface.WriteInput(input);

            //  Fire the event.
            FireProcessInputEvent(new ProcessEventArgs(input));
        });
    }

    /// <summary>
    /// Runs the on UI dispatcher.
    /// </summary>
    /// <param name="action">The action.</param>
    private void RunOnUIDispatcher(Action action)
    {
        if (Dispatcher.CheckAccess())
        {
            //  Invoke the action.
            action();
        }
        else
        {
            Dispatcher.BeginInvoke(action, null);
        }
    }

    /// <summary>
    /// Runs a process.
    /// </summary>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="arguments">The arguments.</param>
    public void StartProcess(string fileName, string arguments)
    {
        StartProcess(new ProcessStartInfo(fileName, arguments));
    }

    /// <summary>
    /// Runs a process.
    /// </summary>
    /// <param name="processStartInfo"><see cref="ProcessStartInfo"/> to pass to the process.</param>
    public void StartProcess(ProcessStartInfo processStartInfo)
    {
        //  Are we showing diagnostics?
        if (ShowDiagnostics)
        {
            WriteOutput("Preparing to run " + processStartInfo.FileName, Color.FromArgb(255, 0, 255, 0));
            if (!string.IsNullOrEmpty(processStartInfo.Arguments))
                WriteOutput(" with arguments " + processStartInfo.Arguments + "." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
            else
                WriteOutput("." + Environment.NewLine, Color.FromArgb(255, 0, 255, 0));
        }

        //  Start the process.
        processInterface.StartProcess(processStartInfo);

        RunOnUIDispatcher(() =>
        {
            //  If we enable input, make the control not read only.
            if (IsInputEnabled)
                IsReadOnly = false;

            //  We're now running.
            IsProcessRunning = true;
                    
        });
    }

    /// <summary>
    /// Stops the process.
    /// </summary>
    public void StopProcess()
    {
        //  Stop the interface.
        processInterface.StopProcess();
    }

    /// <summary>
    /// Fires the console output event.
    /// </summary>
    /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
    private void FireProcessOutputEvent(ProcessEventArgs args)
    {
        //  Fire the event if it is set.
        OnProcessOutput?.Invoke(this, args);
    }

    /// <summary>
    /// Fires the console input event.
    /// </summary>
    /// <param name="args">The <see cref="ProcessEventArgs"/> instance containing the event data.</param>
    private void FireProcessInputEvent(ProcessEventArgs args)
    {
        //  Fire the event if it is set.
        OnProcessInput?.Invoke(this, args);
    }
}