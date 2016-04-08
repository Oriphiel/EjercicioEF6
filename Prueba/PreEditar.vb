Public Class PreEditar

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtID.KeyPress
        OnlyNumbers(e)
    End Sub

    Private Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        ' Tenemos que antes de usar algún valor de algún objeto pasarlo a una variable obligatoriamente, sino ocurrirá un error en EF debido a que no sabrá como
        ' hacer la consulta
        Dim id As Integer = Convert.ToInt32(txtID.Text)
        ' Creando la conexión a la base de datos
        Database = New pruebaEntities()
        Dim actual = Database.fact_cab.SingleOrDefault(Function(o) o.idfact_cab = id)
        If (actual Is Nothing) Then
            MessageBox.Show("No existe esta factura", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            'Tenemos que cerrar la conexión con el servidor, caso contrario la misma se mantendrá abierta
            Database.Dispose()
            txtID.Focus()
            Return
        Else
            'Tenemos que cerrar la conexión con el servidor, caso contrario la misma se mantendrá abierta
            Database.Dispose()
        End If
        Dim nuevo = New Editar(id)
        nuevo.Show()
        Close()
    End Sub
End Class