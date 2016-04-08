Public Class Reporte
    Private _op As Integer

    Public Sub New(opcion As Integer)
        ' Llamada necesaria para el diseñador.
        InitializeComponent()
        _op = opcion
        ' Agregue cualquier inicialización después de la llamada a InitializeComponent().
    End Sub

    Private Sub Reporte_Load(sender As Object, e As EventArgs) Handles Me.Load
        ' Creamos BindingSource para cada una de las tablas
        Dim source As BindingSource = New BindingSource()
        Dim source2 As BindingSource = New BindingSource()
        Dim source3 As BindingSource = New BindingSource()
        Database = New pruebaEntities()
        Dim actual = Database.fact_cab.SingleOrDefault(Function(o) o.idfact_cab = _op)
        ' Aprovechando la versatilidad de EF vamos llenando cada una de sources
        source.DataSource = actual
        source2.DataSource = actual.fact_det.ToList()
        source3.DataSource = actual.usuario
        Dim factura As Factura = New Factura()
        ' Colocamos las sources de acuerdo al orden de las tablas en el reporte
        factura.Database.Tables(0).SetDataSource(source)
        factura.Database.Tables(1).SetDataSource(source2)
        factura.Database.Tables(2).SetDataSource(source3)
        ' Cargamos el visor de Crystal Reports
        CrystalReportViewer1.ReportSource = factura
        CrystalReportViewer1.RefreshReport()
        Database.Dispose()
    End Sub
End Class