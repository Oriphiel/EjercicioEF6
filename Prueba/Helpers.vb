Module Helpers
    ''' <summary>
    ''' Permite capturar el evento de KeyPress y evitar que se escriban caracteres diferentes a los números
    ''' </summary>
    ''' <param name="e"><see cref="KeyPressEventArgs"/> La instancia del evento que contiene los datos.</param>
    Public Sub OnlyNumbers(ByRef e As KeyPressEventArgs)
        If Asc(e.KeyChar) <> 13 AndAlso Asc(e.KeyChar) <> 8 AndAlso Not IsNumeric(e.KeyChar) AndAlso e.KeyChar <> "." Then
            e.Handled = True
        End If
    End Sub


    ''' <summary>
    ''' Permite capturar el evento de KeyPress y evitar que se escriban caracteres diferentes a las letras
    ''' </summary>
    ''' <param name="e"><see cref="KeyPressEventArgs"/> La instancia del evento que contiene los datos.</param>
    Public Sub OnlyLetters(ByRef e As KeyPressEventArgs)
        If Not Char.IsLetter(e.KeyChar) AndAlso Not Char.IsControl(e.KeyChar) AndAlso Not Char.IsWhiteSpace(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub


    Public Database As pruebaEntities
End Module
