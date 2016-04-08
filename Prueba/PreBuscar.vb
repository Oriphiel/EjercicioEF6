Public Class PreBuscar

    Private Sub TextBox1_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtID.KeyPress
        OnlyNumbers(e)
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim id As Integer = Convert.ToInt32(txtID.Text)
        Database = New pruebaEntities()
        Dim actual = Database.fact_cab.SingleOrDefault(Function(o) o.idfact_cab = id)
        If (actual Is Nothing) Then
            MessageBox.Show("No existe esta factura", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Database.Dispose()
            txtID.Focus()
            Return
        End If
        Dim nuevo = New Buscar(id)
        nuevo.Show()
        Close()
    End Sub
End Class