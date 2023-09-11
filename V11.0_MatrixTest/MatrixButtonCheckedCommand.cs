using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace V11._0_MatrixTest
{
    public class MatrixCommand : ICommand
    {

        public MatrixCommand()
        {

        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            
        }
    }
}
