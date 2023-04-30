﻿using System;
using System.Windows.Input;
using SoundDesigner.Lib.CommandRouting.EventArgs;

namespace SoundDesigner.Lib.CommandRouting;

public class RoutableCommand : ICommand
{
    #region Fields

    /// <summary>
    /// The action (or parameterized action) that will be called when the command is invoked.
    /// </summary>
    protected Action Action;

    protected Action<object> ParameterizedAction;
    protected readonly Func<object, bool> CanExecuteFunction;

    #endregion

    #region Construction

    public RoutableCommand(Action<object> parameterizedAction, Func<object, bool> canExecuteDelegate)
    {
        ParameterizedAction = parameterizedAction;
        CanExecuteFunction = canExecuteDelegate;

    }

    public RoutableCommand(Action action, Func<object, bool> canExecuteDelegate)
    {
        Action = action;
        CanExecuteFunction = canExecuteDelegate;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// This method can be used to raise the CanExecuteChanged handler.
    /// This will force WPF to re-query the status of this command directly.
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        if (CanExecuteFunction != null)
            OnCanExecuteChanged();
    }

    #endregion

    #region ICommand Members

    public bool CanExecute(object parameter)
    {
        return CanExecuteFunction(parameter);
    }

    public void Execute(object parameter)
    {
        DoExecute(parameter);
    }

    #endregion

    #region Events

    private EventHandler _internalCanExecuteChanged;

    /// <summary>
    /// Occurs when can execute is changed.
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
        add
        {
            _internalCanExecuteChanged += value;
            CommandManager.RequerySuggested += value;
        }

        remove
        {
            // ReSharper disable DelegateSubtraction
            _internalCanExecuteChanged -= value;
            // ReSharper restore DelegateSubtraction
            CommandManager.RequerySuggested -= value;
        }
    }

    /// <summary>
    /// Occurs when the command is about to execute.
    /// </summary>
    public event CancelCommandEventHandler Executing;

    /// <summary>
    /// Occurs when the command executed.
    /// </summary>
    public event CommandEventHandler Executed;

    #endregion

    #region Protected Members

    /// <summary>
    /// This method is used to walk the delegate chain and well WPF that
    /// our command execution status has changed.
    /// </summary>
    protected virtual void OnCanExecuteChanged()
    {
        EventHandler eCanExecuteChanged = _internalCanExecuteChanged;
        if (eCanExecuteChanged != null)
            eCanExecuteChanged(this, System.EventArgs.Empty);
    }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="param">The param.</param>
    protected void DoExecute(object param)
    {
        //  Invoke the executing command, allowing the command to be cancelled.
        var args =
            new CancelCommandEventArgs { Parameter = param, Cancel = false };

        InvokeExecuting(args);

        //  If the event has been cancelled, bail now.
        if (args.Cancel)
            return;

        //  Call the action or the parameterized action, whichever has been set.
        InvokeAction(param);

        //  Call the executed function.
        InvokeExecuted(new CommandEventArgs { Parameter = param });
    }

    protected void InvokeAction(object param)
    {
        Action theAction = Action;
        Action<object> theParameterizedAction = ParameterizedAction;
        if (theAction != null)
            theAction();
        else if (theParameterizedAction != null)
            theParameterizedAction(param);
    }

    protected void InvokeExecuted(CommandEventArgs args)
    {
        CommandEventHandler executed = Executed;

        //  Call the executed event.
        if (executed != null)
            executed(this, args);
    }

    protected void InvokeExecuting(CancelCommandEventArgs args)
    {
        CancelCommandEventHandler executing = Executing;

        //  Call the executed event.
        if (executing != null)
            executing(this, args);
    }

    #endregion
}
