using ObjCRuntime;
using LibVLCSharp.Shared;
using System.Diagnostics;
using Foundation;

namespace tmp;

public partial class ViewController : NSViewController {
	protected ViewController (NativeHandle handle) : base (handle)
	{
		// This constructor is required if the view controller is loaded from a xib or a storyboard.
		// Do not put any initialization here, use ViewDidLoad instead.
	}

	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();

		
		try
		{
			Core.Initialize(); // No custom path needed - framework is bundled
			
			var libvlc = new LibVLC();
			Debug.WriteLine("libvlc.Changeset " + libvlc.Changeset);
			Debug.WriteLine("libvlc.Version " + libvlc.Version);
		}
		catch (Exception ex)
		{
		}
		
		// Do any additional setup after loading the view.
	}

	public override NSObject RepresentedObject {
		get => base.RepresentedObject;
		set {
			base.RepresentedObject = value;

			// Update the view, if already loaded.
		}
	}
}
