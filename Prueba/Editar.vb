Imports System.Data.Entity.Migrations
Imports System.Globalization

Public Class Editar
    Private _listaDetalle As List(Of fact_det) = New List(Of fact_det)()
    Private _id As Integer

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPrecio.KeyPress
        OnlyNumbers(e)
        If e.KeyChar = "." AndAlso txtPrecio.Text.Contains(".") Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtCantidad_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCantidad.KeyPress
        OnlyNumbers(e)
    End Sub

    Private Sub txtApellido_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtApellido.KeyPress
        OnlyLetters(e)
    End Sub

    Private Sub txtNombre_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtNombre.KeyPress
        OnlyLetters(e)
    End Sub

    Private Sub txtCedula_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtCedula.KeyPress
        OnlyNumbers(e)
    End Sub

    Private Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        ' Al ser cada tabla también una clase, podemos usarla para almacenar datos en memoria, hasta que procedamos a guardar en la base de datos
        Dim detalle As fact_det = New fact_det()
        detalle.Detalle = txtDetalle.Text
        detalle.Cantidad = Convert.ToInt32(txtCantidad.Text)
        detalle.Precio = Decimal.Parse(txtPrecio.Text)
        detalle.Estado = "A"
        detalle.idfact_cab = Convert.ToInt32(lblIDFactura.Text)
        _listaDetalle.Add(detalle)
        FillData(_listaDetalle)
        txtCantidad.Text = ""
        txtDetalle.Text = ""
        txtPrecio.Text = ""
    End Sub

    ''' <summary>
    ''' Llenar los datos del DataGridView
    ''' </summary>
    ''' <param name="lista">De lista.</param>
    Public Sub FillData(ByRef lista As List(Of fact_det))
        dgvDetalle.DataSource = Nothing
        dgvDetalle.Rows.Clear()
        dgvDetalle.ColumnCount = 5
        With dgvDetalle
            .RowHeadersVisible = False
            .Columns(0).HeaderCell.Value = "Detalle"
            .Columns(1).HeaderCell.Value = "Precio"
            .Columns(2).HeaderCell.Value = "Cantidad"
            .Columns(3).HeaderCell.Value = "Estado"
            .Columns(4).HeaderCell.Value = "ID"
            .Columns(4).Name = "ID"
            .Columns(4).Visible = False
        End With
        Dim total As Decimal = 0
        For Each rub In lista
            dgvDetalle.Rows.Add({rub.Detalle, rub.Precio, rub.Cantidad, rub.Estado, rub.idfact_det})
            total += rub.Precio * rub.Cantidad
        Next
        dgvDetalle.Refresh()
        lblTotal.Text = total
    End Sub

    Private Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
        If txtNombre.Text.Length > 0 Then
            Try
                'Iniciamos la base de datos
                Database = New pruebaEntities()
                ' Buscamos la factura escogida
                Dim id As Integer = Convert.ToInt32(lblIDFactura.Text)
                Dim factura As fact_cab = Database.fact_cab.SingleOrDefault(Function(o) o.idfact_cab = id)
                factura.usuario.Nombres = txtNombre.Text
                factura.usuario.Apellidos = txtApellido.Text
                factura.usuario.Cedula = txtCedula.Text
                factura.Estado = "A"
                factura.fecha = Format(dtpFecha.Value, "yyyy-MM-dd")
                factura.total = Decimal.Parse(lblTotal.Text)
                factura.fpago = "Efectivo"
                ' Accedemos a cada uno de las instancias de la clase en la lista y procedemos a prepararlas para guardarlas
                For Each detalle In _listaDetalle
                    Database.fact_det.AddOrUpdate(detalle)
                Next
                Database.fact_cab.AddOrUpdate(factura)
                ' Esta opción realiza la escritura en la base de datos de los datos ingresados
                Database.SaveChanges()
                Database.Dispose()
                Close()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End If
    End Sub

    Private Sub btnBorrar_Click(sender As Object, e As EventArgs) Handles btnBorrar.Click
        If dgvDetalle.Rows.Count > 0 Then
            Dim idDetalle As Integer = dgvDetalle.CurrentRow.Cells("ID").Value
            Dim id As Integer = dgvDetalle.CurrentRow.Index
            _listaDetalle.RemoveAt(id)
            dgvDetalle.Rows.RemoveAt(id)
            FillData(_listaDetalle)
            Database = New pruebaEntities()
            ' Para poder borrar necesitamos buscar la entidad primero y luego eliminarla
            Dim borrar As fact_det = Database.fact_det.Find(idDetalle)
            Database.fact_det.Remove(borrar)
            Database.SaveChanges()
            Database.Dispose()
        End If
    End Sub

    Private Sub Editar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Application.CurrentCulture = New CultureInfo("EN-US")
        ' Con esto se crea una nueva conexión a la base de datos
        Database = New pruebaEntities()
        Dim actual As fact_cab = Database.fact_cab.Find(_id)
        txtApellido.Text = actual.usuario.Apellidos
        txtNombre.Text = actual.usuario.Nombres
        txtCedula.Text = actual.usuario.Cedula
        lblIDFactura.Text = actual.idfact_cab
        dtpFecha.Text = actual.fecha.ToString()
        _listaDetalle = actual.fact_det.ToList()
        FillData(_listaDetalle)
        ' Con esto cortamos la conexión con la base de datos
        Database.Dispose()
    End Sub

    Public Sub New(id As Integer)
        ' Llamada necesaria para el diseñador.
        InitializeComponent()
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
        _id = id
    End Sub
End Class