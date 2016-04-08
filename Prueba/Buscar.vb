Public Class Buscar
    Private _op As Integer

    Public Sub New(op As Integer)
        ' Llamada necesaria para el diseñador.
        InitializeComponent()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        _op = op
    End Sub

    Private Sub Buscar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Database = New pruebaEntities()
            ' Buscamos en la tabla fact_cab y si encuentra un registro de acuerdo a la condición, en este caso encuentra un id con el mismo valor que _op, lo devuelve, caso
            ' contrario sera Ninguno
            Dim actual = Database.fact_cab.SingleOrDefault(Function(o) o.idfact_cab = _op)
            lblIDFactura.Text = actual.idfact_cab
            ' Esto es lo maravilloso de EF, se puede acceder a los registros de las tablas relacionadas desde la actual tabla
            txtNombre.Text = actual.usuario.Nombres
            txtApellido.Text = actual.usuario.Apellidos
            txtCedula.Text = actual.usuario.Cedula
            dtpFecha.Text = actual.fecha.ToString()
            FillData(actual.fact_det.ToList())
            Database.Dispose()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Llenar los datos del DataGridView
    ''' </summary>
    ''' <param name="lista">De lista.</param>
    Public Sub FillData(ByRef lista As List(Of fact_det))
        dgvDetalle.DataSource = Nothing
        dgvDetalle.Rows.Clear()
        dgvDetalle.ColumnCount = 4
        With dgvDetalle
            .RowHeadersVisible = False
            .Columns(0).HeaderCell.Value = "Detalle"
            .Columns(1).HeaderCell.Value = "Precio"
            .Columns(2).HeaderCell.Value = "Cantidad"
            .Columns(3).HeaderCell.Value = "Estado"
        End With
        Dim total As Decimal = 0
        For Each rub In lista
            dgvDetalle.Rows.Add({rub.Detalle, rub.Precio, rub.Cantidad, rub.Estado})
            total += rub.Precio * rub.Cantidad
        Next
        dgvDetalle.Refresh()
        lblTotal.Text = total
    End Sub

    Private Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        Dim reporte = New Reporte(Convert.ToInt32(lblIDFactura.Text))
        reporte.Show()
    End Sub
End Class