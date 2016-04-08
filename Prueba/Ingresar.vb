Imports System.Data.Entity
Imports System.Data.Entity.Migrations
Imports System.Globalization

Public Class Ingresar
    Public _listaDetalle As List(Of fact_det) = New List(Of fact_det)()

    Private Sub Ingresar_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            Application.CurrentCulture = New CultureInfo("EN-US")
            dtpFecha.Value = DateTime.Now
            ' Con esto se crea una nueva conexión a la base de datos
            Database = New pruebaEntities()
            ' Al conectarnos a la base de datos, podemos acceder directamente a la table a través del modelo de EF, y podemos trabajar con ella
            lblIDFactura.Text = Database.fact_cab.Count() + 1
            ' Con esto cortamos la conexión con la base de datos
            Database.Dispose()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try
    End Sub

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
        If txtNombre.Text.Length > 0 Then
            Try
                'Iniciamos la base de datos
                Database = New pruebaEntities()
                ' Creamos un nuevo usuario
                Dim usuario = New usuario
                usuario.Nombres = txtNombre.Text
                usuario.Apellidos = txtApellido.Text
                usuario.Cedula = txtCedula.Text
                'Creamos una nueva cabecera de factura
                Dim factura = New fact_cab
                factura.Estado = "A"
                factura.fecha = Format(dtpFecha.Value, "yyyy-MM-dd")
                factura.total = Decimal.Parse(lblTotal.Text)
                factura.fpago = "Efectivo"
                factura.idUsuario = usuario.idusuario
                ' Deja preparado los datos para ser guardados en la base de datos
                Database.usuario.AddOrUpdate(usuario)
                Database.fact_cab.AddOrUpdate(factura)
                ' Accedemos a cada uno de las instancias de la clase en la lista y procedemos a prepararlas para guardarlas
                For Each detalle In _listaDetalle
                    detalle.idfact_cab = factura.idfact_cab
                    Database.fact_det.AddOrUpdate(detalle)
                Next
                ' Esta opción realiza la escritura en la base de datos de los datos ingresados
                Database.SaveChanges()
                Database.Dispose()
                Close()
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try
        End If
    End Sub
End Class