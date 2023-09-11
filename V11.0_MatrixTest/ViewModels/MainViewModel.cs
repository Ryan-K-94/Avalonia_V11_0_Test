using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using static V11._0_MatrixTest.ViewModels.MainViewModel;

namespace V11._0_MatrixTest.ViewModels;

public class MainViewModel : ViewModelBase
{
    private Matrix matrix;
    private int tabCtrlIndex;
    private InOut bindingTest;

    public Matrix PatchMatrix
    {
        get { return matrix; }
        set { this.RaiseAndSetIfChanged(ref matrix, value); }
    }

    public int TabCtrlIndex
    {
        get => tabCtrlIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref tabCtrlIndex, value);

            if (tabCtrlIndex == 1)
            {
                CreateMatrix(50);
            }
        }
    }

    public InOut BindingTest
    {
        get { return bindingTest; }
        set { this.RaiseAndSetIfChanged(ref bindingTest, value); }
    }

    public MainViewModel() 
    {
        CreateMatrix(0);
        BindingTest = InOut.In;
    }

    private PatchMatrixColumn CreateMatrixColumn(SatBoxColours colour, string satBoxName, int satBoxId, int satBoxChCount, int inputChCount)
    {
        var matrix = new PatchMatrixColumn();

        matrix.ColumnHeader = new List<List<string>>();
        matrix.Column = new List<List<MatrixItem>>();
        matrix.SatBoxName = satBoxName;
        matrix.SatBoxId = satBoxId;
        matrix.SatColour = colour;

        matrix.ColumnHeader.Add(new List<string>());

        for (int ch = 0; ch < satBoxChCount; ch++)
        {
            matrix.ColumnHeader[0].Add((ch + 1).ToString());
        }

        for (int y = 0; y < inputChCount; y++)
        {
            matrix.Column.Add(new List<MatrixItem>());

            for (int x = 0; x < satBoxChCount; x++)
            {
                matrix.Column[y].Add(new MatrixItem() { Command = new MatrixCommand() });
            }
        }

        return matrix;
    }

    private PatchMatrixRow CreateMatrixRow(int rowCh, string rowName)
    {
        var matrix = new PatchMatrixRow();

        matrix.MatrixRowCh = rowCh.ToString();
        matrix.MatrixRowName = rowName;

        return matrix;
    }

    private async void CreateMatrix(int delay)
    {
        PatchMatrix = new Matrix();
        PatchMatrix.MatrixColumn = new ObservableCollection<PatchMatrixColumn>();
        PatchMatrix.MatrixRow = new ObservableCollection<PatchMatrixRow>();

        await Task.Delay(delay);

        for (int ch = 0; ch < 64; ch++)
        {
            PatchMatrix.MatrixRow.Add(CreateMatrixRow(ch, ch.ToString()));
        }

        for (int sat = 0; sat < 4; sat++)
        {
            PatchMatrix.MatrixColumn.Add(CreateMatrixColumn(SatBoxColours.Red, sat.ToString(), sat, 16, 64));
        }
    }

    public enum InOut
    {
        [Description("Input Patch")]
        In = 0,

        [Description("Output Patch")]
        Out = 1,
    }

    public enum SatBoxColours
    {
        Brown,
        Red,
        Orange,
        Yellow,
        Green,
        Blue,
        Purple,
        Gray,
        White
    }

    public class Matrix : ReactiveObject
    {
        private ObservableCollection<PatchMatrixColumn>? matrixColumn;
        private ObservableCollection<PatchMatrixRow>? matrixRow;

        public ObservableCollection<PatchMatrixRow> MatrixRow
        {
            get { return matrixRow!; }
            set { this.RaiseAndSetIfChanged(ref matrixRow, value); }
        }

        public ObservableCollection<PatchMatrixColumn> MatrixColumn
        {
            get { return matrixColumn!; }
            set { this.RaiseAndSetIfChanged(ref matrixColumn, value); }
        }
    }

    public class PatchMatrixRow
    {
        public string? MatrixRowCh { get; set; }
        public string? MatrixRowName { get; set; }
    }

    public class PatchMatrixColumn : ReactiveObject
    {
        private bool isPaneOpen = true;
        private double openPaneLength;

        // Matrix view members, used by both ItemsRepeaters and their child controls.
        public double OpenPaneLength
        {
            get => openPaneLength;
            set
            {
                this.RaiseAndSetIfChanged(ref openPaneLength, value);
            }
        }
        public bool IsPaneOpen
        {
            get => isPaneOpen;
            set
            {
                this.RaiseAndSetIfChanged(ref isPaneOpen, value);
            }
        }

        // Column data
        public string? SatBoxName { get; set; }
        public int SatBoxId { get; set; }
        public SatBoxColours SatColour { get; set; }
        public List<List<MatrixItem>>? Column { get; set; }
        public List<List<string>>? ColumnHeader { get; set; }
    }

    public class MatrixItem : ReactiveObject
    {
        private bool isAssigned;
        private InOut isAssignedTooInOut;
        private MatrixButtonSate buttonState;

        public ICommand? Command { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public int SatBoxId { get; set; }

        public bool IsAssigned
        {
            get => isAssigned;
            set { this.RaiseAndSetIfChanged(ref isAssigned, value); }
        }

        public InOut IsAssignedTooInOut
        {
            get { return isAssignedTooInOut; }
            set { this.RaiseAndSetIfChanged(ref isAssignedTooInOut, value); }
        }

        public MatrixButtonSate ButtonState
        {
            get { return buttonState; }
            set { this.RaiseAndSetIfChanged(ref buttonState, value); }
        }
    }

    public enum MatrixButtonSate
    {
        IsNotActive,
        IsActive,
        IsPointerOver
    }
}

public class MatrixToggleBtnColourConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        switch ((InOut)value)
        {
            default:
                // Should never get here.
                return Brush.Parse("#ED1A69");

            case InOut.In:
                // Solotech Pink
                return Brush.Parse("#ED1A69");

            case InOut.Out:
                // Turquoise 
                return Brush.Parse("#03A38E");
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

