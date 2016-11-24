#define DISABLED
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

public class AN_ManifestManagerWindow : EditorWindow {
	private int toolbarButtonIndex = 0;
	private Vector2 scrollPos = Vector2.zero;

	private AN_BaseTemplate _parentTemplate = null;

	private List<string> _permissionsStrings = null;
	private List<AN_ManifestPermission> _permissionsArray = null;


	GUIContent SdkVersion   = new GUIContent("Plugin Version [?]", "This is Plugin version.  If you have problems or compliments please include this so we know exactly what version to look out for.");
	GUIContent SupportEmail = new GUIContent("Support [?]", "If you have any technical quastion, feel free to drop an e-mail");

	void OnGUI() {
		GUILayout.Label ("Android Manifest Manager", EditorStyles.boldLabel);

		if (!AN_ManifestManager.HasManifest ) {
			GUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			GUILayout.Label("You have NO AndroidManifest file in your project!", GUILayout.Width(300.0f));
			EditorGUILayout.Space();
			GUILayout.EndHorizontal();

			EditorGUILayout.Space();
			GUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button("Create Default Manifest", new GUILayoutOption[]{GUILayout.Width(200.0f), GUILayout.Height(50.0f)})) {
				AN_ManifestManager.CreateDefaultManifest();
			}
			EditorGUILayout.Space();
			GUILayout.EndHorizontal();
		} else {


			string[] toolbarButtons = new string[]{"Manifest", "Application", "Permissions"};
			toolbarButtonIndex = GUILayout.Toolbar(toolbarButtonIndex, toolbarButtons, new GUILayoutOption[]{GUILayout.Height(30.0f)});

			switch (toolbarButtons[toolbarButtonIndex]) {
			case "Manifest" : {
				AN_ManifestTemplate manifest = AN_ManifestManager.GetManifest();

				if (manifest != null) {
					GUILayout.Label ("Values", EditorStyles.boldLabel);
					foreach (string key in manifest.Values.Keys) {
						EditorGUILayout.BeginHorizontal();
						
						GUILayout.Label(key);
						if (key.Equals("xmlns:android") ||
						    key.Equals("android:installLocation") ||
						    key.Equals("package") ||
						    key.Equals("android:versionName") ||
						    key.Equals("android:versionCode") ||
						    key.Equals("android:theme")) {
							
							GUI.enabled = false;
							GUILayout.TextField(AN_ManifestManager.GetManifest().Values[key], GUILayout.Width(300.0f));
						} else {
							GUI.enabled = true;
							
							string input = AN_ManifestManager.GetManifest().Values[key];
							EditorGUI.BeginChangeCheck();
							input = GUILayout.TextField(AN_ManifestManager.GetManifest().Values[key], GUILayout.Width(276.0f));
							if(EditorGUI.EndChangeCheck()) {
								AN_ManifestManager.GetManifest().SetValue(key, input);
								return;
							}
							
							if(GUILayout.Button("X", GUILayout.Width(20.0f))) {
								AN_ManifestManager.GetManifest().RemoveValue(key);
								return;
							}
						}
						GUI.enabled = true;
						EditorGUILayout.EndHorizontal();
					}
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					if (GUILayout.Button("Add Value", GUILayout.Width(100.0f))) {
						AddValueDialog(AN_ManifestManager.GetManifest());
					}
					EditorGUILayout.Space();
					EditorGUILayout.EndHorizontal();
					
					GUILayout.Label ("Properties", EditorStyles.boldLabel);
					DrawProperties(AN_ManifestManager.GetManifest());
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					if (GUILayout.Button("Add Property", GUILayout.Width(100.0f))) {
						AddPropertyDialog(AN_ManifestManager.GetManifest());
					}
					EditorGUILayout.Space();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Space();
					
					EditorGUILayout.Space();
					if(GUILayout.Button("Save Manifest", GUILayout.Height(22.0f))) {
						AN_ManifestManager.SaveManifest();
					}
				} else {
					EditorGUILayout.HelpBox("Selected build platform DOESN'T support AndroidManifest.xml file", MessageType.Info);
				}
			} break;
			case "Application" : {
				AN_ManifestTemplate manifest = AN_ManifestManager.GetManifest();
				
				if (manifest != null) {
					scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (position.width), GUILayout.Height (position.height - 50f));
					
					GUILayout.Label ("Values", EditorStyles.boldLabel);
					foreach (string key in manifest.ApplicationTemplate.Values.Keys) {
						EditorGUILayout.BeginHorizontal();
						
						GUILayout.Label(key);
						
						string input = AN_ManifestManager.GetManifest().ApplicationTemplate.Values[key];
						EditorGUI.BeginChangeCheck();
						input = GUILayout.TextField(AN_ManifestManager.GetManifest().ApplicationTemplate.Values[key], GUILayout.Width(200.0f));
						if(EditorGUI.EndChangeCheck()) {
							AN_ManifestManager.GetManifest().ApplicationTemplate.SetValue(key, input);
							return;
						}
						
						if(GUILayout.Button("X", GUILayout.Width(20.0f))) {
							AN_ManifestManager.GetManifest().ApplicationTemplate.RemoveValue(key);
							return;
						}
						
						EditorGUILayout.EndHorizontal();
					}
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					if (GUILayout.Button("Add Value", GUILayout.Width(100.0f))) {
						AddValueDialog(AN_ManifestManager.GetManifest().ApplicationTemplate);
					}
					EditorGUILayout.Space();
					EditorGUILayout.EndHorizontal();
					
					GUILayout.Label ("Activities", EditorStyles.boldLabel);
					
					int launcherActivities = 0;
					foreach (int id in AN_ManifestManager.GetManifest().ApplicationTemplate.Activities.Keys) {
						AN_ActivityTemplate activity = AN_ManifestManager.GetManifest().ApplicationTemplate.Activities[id];
						
						if (activity.IsLauncher) {
							launcherActivities++;
						}
						
						EditorGUILayout.BeginVertical(GUI.skin.box);
						EditorGUILayout.BeginHorizontal();
						activity.IsOpen = EditorGUILayout.Foldout(activity.IsOpen, activity.Name);
						if(GUILayout.Button("X", GUILayout.Width(20.0f))) {
							AN_ManifestManager.GetManifest().ApplicationTemplate.RemoveActivity(activity);
							return;
						}
						EditorGUILayout.EndHorizontal();
						
						if (activity.IsOpen) {
							EditorGUILayout.BeginVertical();
							
							bool isLauncher = activity.IsLauncher;
							EditorGUI.BeginChangeCheck();
							isLauncher = EditorGUILayout.Toggle("Is Launcher", activity.IsLauncher);
							if (EditorGUI.EndChangeCheck()) {
								activity.SetAsLauncher(isLauncher);
							}
							
							foreach (string k in activity.Values.Keys) {
								EditorGUILayout.BeginHorizontal();
								
								GUILayout.Label(k);
								EditorGUILayout.Space();
								
								string input = activity.Values[k];
								EditorGUI.BeginChangeCheck();
								
								if (k.Equals("android:name")) {
									input = GUILayout.TextField(activity.Values[k], GUILayout.Width(224.0f));
								} else {
									input = GUILayout.TextField(activity.Values[k], GUILayout.Width(200.0f));
								}
								
								if(EditorGUI.EndChangeCheck()) {
									activity.SetValue(k, input);
									return;
								}
								
								if (!k.Equals("android:name")) {
									if(GUILayout.Button("X", GUILayout.Width(20.0f))) {
										activity.RemoveValue(k);
										return;
									}
								}
								
								EditorGUILayout.EndHorizontal();
								EditorGUILayout.Space();
							}
							
							DrawProperties(activity);
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.Space();
							if (GUILayout.Button("Add Value", GUILayout.Width(100.0f))) {
								AddValueDialog(activity);
							}
							if (GUILayout.Button("Add Property", GUILayout.Width(100.0f))) {
								AddPropertyDialog(activity);
							}
							EditorGUILayout.Space();
							EditorGUILayout.EndHorizontal();
							EditorGUILayout.Space();
							
							EditorGUILayout.EndVertical();
						}
						EditorGUILayout.EndVertical();
					}
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					if (GUILayout.Button("Add Activity", GUILayout.Width(100.0f))) {
						AddPermissionDialog dlg = EditorWindow.CreateInstance<AddPermissionDialog>();
						dlg.onClose += OnPermissionDlgClose;
						dlg.onAddClick += OnAddActivityClick;
						
						#if UNITY_5
						dlg.titleContent.text = "Add Activity";
						#else
						dlg.title = "Add Activity";
						#endif
						
						dlg.ShowAuxWindow();
					}
					EditorGUILayout.Space();
					EditorGUILayout.EndHorizontal();
					
					if (launcherActivities > 1) {
						EditorGUILayout.HelpBox("There is MORE THAN ONE Launcher Activity in Manifest", MessageType.Warning);
					} else if (launcherActivities < 1){
						EditorGUILayout.HelpBox("There is NO Launcher Activities in Manifest", MessageType.Warning);
					}
					
					GUILayout.Label ("Properties", EditorStyles.boldLabel);
					DrawProperties(AN_ManifestManager.GetManifest().ApplicationTemplate);
					
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					if (GUILayout.Button("Add Property", GUILayout.Width(100.0f))) {
						AddPropertyDialog(AN_ManifestManager.GetManifest().ApplicationTemplate);
					}
					EditorGUILayout.Space();
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					EditorGUILayout.EndScrollView();
				} else {
					EditorGUILayout.HelpBox("Selected build platform DOESN'T support AndroidManifest.xml file", MessageType.Info);
				}				
			} break;
			case "Permissions" : {
				AN_ManifestTemplate manifest = AN_ManifestManager.GetManifest();
				
				if (manifest != null) {
					scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width (position.width), GUILayout.Height (position.height - 50f));
					EditorGUILayout.BeginVertical();
					
					foreach (AN_PropertyTemplate permission in AN_ManifestManager.GetManifest().Permissions) {
						EditorGUILayout.BeginHorizontal(GUI.skin.box);
						EditorGUILayout.LabelField(permission.Values["android:name"]);
						if(GUILayout.Button("X", GUILayout.Width(20.0f))) {
							AN_ManifestManager.GetManifest().RemovePermission(permission);
							return;
						}
						EditorGUILayout.EndHorizontal();
					}
					
					EditorGUILayout.BeginHorizontal();
					if(GUILayout.Button("Add Android Permission")) {
						GenericMenu permissionsMenu = new GenericMenu();
						foreach (string pStr in PermissionsStrings) {
							permissionsMenu.AddItem(new GUIContent(pStr), false, SelectPermission, pStr);
						}
						permissionsMenu.ShowAsContext();
					}
					
					if (GUILayout.Button("Add Other Permission")) {
						AddPermissionDialog dlg = EditorWindow.CreateInstance<AddPermissionDialog>();
						dlg.onClose += OnPermissionDlgClose;
						dlg.onAddClick += OnAddPermissionClick;
						
						#if UNITY_5
						dlg.titleContent.text = "Add Permission";
						#else
						dlg.title = "Add Permission";
						#endif
						
						
						dlg.ShowAuxWindow();
					}
					EditorGUILayout.EndHorizontal();
					
					EditorGUILayout.Space();
					EditorGUILayout.EndVertical();
					EditorGUILayout.EndScrollView();
				} else {
					EditorGUILayout.HelpBox("Selected build platform DOESN'T support AndroidManifest.xml file", MessageType.Info);
				}				
			} break;
			default: break;
			}


			AboutGUI();
		}
	}

	private void AboutGUI() {
		EditorGUILayout.Space();
		EditorGUILayout.HelpBox("About the Plugin", MessageType.None);
		EditorGUILayout.Space();
		
		SelectableLabelField(SdkVersion, AMMSettings.VERSION_NUMBER);
		SelectableLabelField(SupportEmail, "stans.assets@gmail.com");


		#if DISABLED
		
		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Note: ", EditorStyles.boldLabel);
		EditorGUILayout.LabelField("This version of Android Manifest Manager designed ");
		EditorGUILayout.LabelField("for the Stan's Assets plugins internal use only.   ");
		EditorGUILayout.LabelField("If you want to use Android Manifest Manager");
		EditorGUILayout.LabelField("for your project needs, please, ");
		EditorGUILayout.LabelField("purchase a copy of Android Manifest Manager plugin.");
		
		
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		
		if(GUILayout.Button("Documentation",  GUILayout.Width(150))) {
			Application.OpenURL("https://goo.gl/V1MJnX");
		}
		
		if(GUILayout.Button("Purchase",  GUILayout.Width(150))) {
			Application.OpenURL("https://goo.gl/YFZaue");
		}
		
		EditorGUILayout.EndHorizontal();

		#endif
	}

	private void SelectableLabelField(GUIContent label, string value) {
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField(label, GUILayout.Width(180), GUILayout.Height(16));
		EditorGUILayout.SelectableLabel(value, GUILayout.Height(16));
		EditorGUILayout.EndHorizontal();
	}

	private void DrawProperties(AN_BaseTemplate parent) {
		foreach (string key in parent.Properties.Keys) {
			foreach (AN_PropertyTemplate property in parent.Properties[key]) {

				if(parent is AN_ActivityTemplate) {
					AN_ActivityTemplate activity = parent as AN_ActivityTemplate;
					if (activity.IsLauncherProperty(property)) {
						continue;
					}
				}

				EditorGUILayout.Space ();
				EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(27.0f));
				EditorGUILayout.BeginHorizontal ();

				if (property.Values.ContainsKey("android:name")) {
					property.IsOpen = EditorGUILayout.Foldout(property.IsOpen, "[" + property.Tag + "] " + property.Values["android:name"]);
				} else {
					if (key.Equals("intent-filter")) {
						property.IsOpen = EditorGUILayout.Foldout(property.IsOpen, "[" + property.Tag + "] " + property.GetIntentFilterName(property));
					} else {
						property.IsOpen = EditorGUILayout.Foldout(property.IsOpen, "[" + property.Tag + "]");
					}
				}

				if (GUILayout.Button ("X", GUILayout.Width(20.0f))) {
					parent.RemoveProperty(property);
					return;
				}
				EditorGUILayout.EndHorizontal ();
				
				if (property.IsOpen) {
					EditorGUILayout.BeginVertical();
					
					foreach (string k in property.Values.Keys) {
						EditorGUILayout.Space();
						EditorGUILayout.BeginHorizontal();
						
						GUILayout.Label(k);
						EditorGUILayout.Space();

						string input = property.Values[k];
						EditorGUI.BeginChangeCheck();
						if (k.Equals("android:name")) {
							input = GUILayout.TextField(property.Values[k], GUILayout.Width(224.0f));
						} else {
							input = GUILayout.TextField(property.Values[k], GUILayout.Width(200.0f));
						}
						if(EditorGUI.EndChangeCheck()) {
							property.SetValue(k, input);
							return;
						}

						if (GUILayout.Button ("X", GUILayout.Width(20.0f))) {
							property.RemoveValue(k);
							return;
						}
						
						EditorGUILayout.EndHorizontal();
					}
					
					DrawProperties(property);
					EditorGUILayout.Space();
					EditorGUILayout.EndVertical();

					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.Space();
					if (GUILayout.Button("Add Value", GUILayout.Width(100.0f))) {
						AddValueDialog(property);
					}
					if (GUILayout.Button("Add Property", GUILayout.Width(100.0f))) {
						AddPropertyDialog(property);
					}
					EditorGUILayout.Space();
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.Space();
				}
				EditorGUILayout.EndVertical();
			}
		}
	}

	private void OnAddActivityClick(string activityName) {
		AN_ActivityTemplate newActivity = new AN_ActivityTemplate(false, activityName);
		newActivity.SetValue("android:name", activityName);
		newActivity.IsOpen = true;
		AN_ManifestManager.GetManifest().ApplicationTemplate.AddActivity(newActivity);
	}

	private void OnAddPermissionClick(string permission) {
		AN_PropertyTemplate property = new AN_PropertyTemplate ("uses-permission");
		property.SetValue ("android:name", permission);
		AN_ManifestManager.GetManifest ().AddPermission(property);
	}

	private void OnPermissionDlgClose(object dialog) {
		AddPermissionDialog dlg = dialog as AddPermissionDialog;
		dlg.onClose -= OnPermissionDlgClose;
		dlg.onAddClick -= OnAddPermissionClick;
	}

	private void SelectPermission(object data) {
		AN_PropertyTemplate newPermission = new AN_PropertyTemplate ("uses-permission");
		newPermission.SetValue ("android:name", data.ToString ());
		AN_ManifestManager.GetManifest ().AddPermission (newPermission);
	}

	private void AddValueDialog(AN_BaseTemplate parent) {
		_parentTemplate = parent;

		AddValueDialog dialog = EditorWindow.CreateInstance<AddValueDialog>();
		dialog.onAddClick += OnAddValueClick;
		dialog.onClose += OnValueDlgClose;

		#if UNITY_5
		dialog.titleContent.text = "Add Value";
		#else
		dialog.title = "Add Value";
		#endif

		dialog.ShowAuxWindow();
	}

	private void AddPropertyDialog(AN_BaseTemplate parent) {
		_parentTemplate = parent;

		AddPropertyDialog dialog = EditorWindow.CreateInstance<AddPropertyDialog>();
		dialog.onAddClick += OnAddPropertyClick;
		dialog.onClose += OnPropertyDlgClose;

		#if UNITY_5
		dialog.titleContent.text = "Add Property";
		#else
		dialog.title = "Add Property";
		#endif

		dialog.ShowAuxWindow();
	}

	private void OnAddValueClick(string key, string value) {
		if (_parentTemplate is AN_ActivityTemplate) {
			if (key.Equals("android:name")) {
				return;
			}
		}
		_parentTemplate.SetValue (key, value);
	}

	private void OnValueDlgClose(object dialog) {
		_parentTemplate = null;

		AddValueDialog dlg = dialog as AddValueDialog;
		dlg.onAddClick -= OnAddValueClick;
		dlg.onClose -= OnValueDlgClose;
	}

	private void OnAddPropertyClick(string tag) {
		AN_PropertyTemplate property = new AN_PropertyTemplate (tag);
		_parentTemplate.AddProperty (tag, property);
	}

	private void OnPropertyDlgClose(object dialog) {
		_parentTemplate = null;

		AddPropertyDialog dlg = dialog as AddPropertyDialog;
		dlg.onAddClick -= OnAddPropertyClick;
		dlg.onClose -= OnValueDlgClose;

	}

	private List<AN_ManifestPermission> PermissionsArray{
		get {
			AN_ManifestPermission[] permissions = (AN_ManifestPermission[])Enum.GetValues(typeof(AN_ManifestPermission));
			_permissionsArray = new List<AN_ManifestPermission>(permissions);

			return _permissionsArray;
		}
	}

	private List<string> PermissionsStrings {
		get {
			_permissionsStrings = new List<string>();
			foreach (AN_ManifestPermission p in PermissionsArray) {
				_permissionsStrings.Add("android.permission." + p.ToString());
			}
			return _permissionsStrings;
		}
	}
}
