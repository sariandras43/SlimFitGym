using SlimFitGym_Mobile.Services;

namespace SlimFitGym_Mobile.Components.Pages;

public partial class QrScanner : ContentPage
{
    public QrScanner(CameraService service)
	{
		InitializeComponent();
		this.BindingContext = service;
        service.qrGrid = qrGrid;
    }
}