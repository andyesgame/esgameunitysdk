using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEditor.Android;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class ESGameBuildPostProcessor : IPostGenerateGradleAndroidProject
{
    public static string fbToken = "";
    private static PBXProject _project;

    [PostProcessBuildAttribute]
    public static void OnPostprocessBuildAppsflyer(BuildTarget target, string pathToBuiltProject)
    {

        if (target == BuildTarget.iOS)
        {
            string configPath = "Assets/esgconfig" + "/esg_config.json";
            string configContent = File.ReadAllText(configPath);
            JObject data = JObject.Parse(configContent);
            string fbId = (string)data.GetValue("fbId");
            string fbClientToken = (string)data.GetValue("fbClientToken");
            string gplistPath = "Assets/Plugins/iOs" + "/GoogleService-Info.plist";
            PlistDocument ggPlist = new PlistDocument();
            ggPlist.ReadFromString(File.ReadAllText(gplistPath));
            string reserverClientId = ggPlist.root.values["REVERSED_CLIENT_ID"].AsString();
            Debug.Log("Info.plist reserverClientId " + reserverClientId);
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));

            PlistElementDict rootDict = plist.root;
            rootDict.SetBoolean("AppsFlyerShouldSwizzle", true);
            rootDict.SetString("NSCameraUsageDescription", "Chúng tôi sử dụng camera để update avatar ingame và tính năng báo lỗi.");
            rootDict.SetString("NSUserTrackingUsageDescription", "$(PRODUCT_NAME) cần xin quyền AppTrackingTransparency để giúp bạn trải nghiệm trò chơi tốt hơn và chia sẻ nội dung cập nhật mới nhất thông qua quảng cáo cá nhân");

            PlistElementArray array = rootDict.CreateArray("SKAdNetworkItems");
            PlistElementDict dict = array.AddDict();
            dict.SetString("SKAdNetworkIdentifier", "v9wttpbfk9.skadnetwork");
            PlistElementDict dict1 = array.AddDict();
            dict1.SetString("SKAdNetworkIdentifier", "n38lu8286q.skadnetwork");
            rootDict.SetBoolean("AppsFlyerShouldSwizzle", true);
           
            rootDict.SetString("FacebookAppID", fbId);
            rootDict.SetString("FacebookClientToken", fbClientToken);

            if (!rootDict.values.ContainsKey("CFBundleURLTypes"))
            {
                PlistElementArray urlType = rootDict.CreateArray("CFBundleURLTypes");
                PlistElementDict tmp = urlType.AddDict();
                PlistElementArray arrSchema = tmp.CreateArray("CFBundleURLSchemes");
                arrSchema.AddString("fb"+fbId);
                arrSchema.AddString(reserverClientId);
            }
            else
            {
                PlistElementArray urlType = rootDict.values["CFBundleURLTypes"].AsArray();
                PlistElementDict tmp = urlType.values[0].AsDict();
                PlistElementArray arrSchema = tmp.values["CFBundleURLSchemes"].AsArray();
                List<PlistElement> schemaArray = arrSchema.values;
                Debug.Log("schemaArray "+ schemaArray.Count);
                if (schemaArray.Count == 0)
                {
                    PlistElement elementFb = new PlistElementString("fb" + fbId);
                    schemaArray.Add(elementFb);
                    PlistElement element = new PlistElementString(reserverClientId);
                    schemaArray.Add(element);
                }
                else
                {
                    var size = schemaArray.Count;
                    bool matchReserverClientId = false;
                    bool matchFbId = false;
                    string fbSchema = "fb" + fbId;
                    for (var i = 0; i < size; i++)
                    {
                        if (schemaArray[i].AsString().Equals(reserverClientId))
                        {
                            matchReserverClientId = true;
                        }
                        if (schemaArray[i].AsString().Equals(fbSchema))
                        {
                            matchFbId = true;
                            break;
                        }
                    }
                    if (!matchFbId)
                    {
                        PlistElement element = new PlistElementString(fbSchema);
                        schemaArray.Add(element);
                    }
                    if (!matchReserverClientId)
                    {
                        PlistElement element = new PlistElementString(reserverClientId);
                        schemaArray.Add(element);
                    }
                }
            }
            File.WriteAllText(plistPath, plist.WriteToString());
            PBXProject proj = new PBXProject();
            string pbxFilename = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
            proj.ReadFromFile(pbxFilename);

            string guid = getTargetId(proj);
            proj.AddFrameworkToProject(guid, "AppTrackingTransparency.framework", false);

            proj.WriteToFile(pbxFilename);
            Debug.Log("Info.plist updated with AppsFlyerShouldSwizzle");
        }

    }

    private static string getGGServiceAndroidPath(string path)
    {
        string finalPath = "";
#if UNITY_2019_3_OR_NEWER
        if (path.Contains("gradleOut"))
        {
            string dataPath = Application.dataPath;
            Debug.Log("content " + Application.dataPath);
            string tmp = dataPath.Remove(dataPath.IndexOf("Assets"));
            Debug.Log("tmp " + tmp);
            finalPath = tmp + path.Remove(path.IndexOf("unityLibrary")) + "launcher/google-services.json";
        }
        else
        {
            finalPath = path.Remove(path.IndexOf("unityLibrary"))+ "launcher/google-services.json";
        }
#else
        if (path.Contains("gradleOut"))
        {
            string dataPath = Application.dataPath;
            Debug.Log("content " + Application.dataPath);
            string tmp = dataPath.Remove(dataPath.IndexOf("Assets"));
            Debug.Log("tmp " + tmp);
        
            finalPath = tmp + path + "/google-services.json";
        }
        else
        {
            finalPath = path.Remove(path.IndexOf("unityLibrary"))+ "/google-services.json";
        }
        
        
#endif
        Debug.Log("finalPath " + finalPath);
        return finalPath;
    }

    private static string getTargetId(PBXProject project)
    {
        string targetId = "";
#if UNITY_2019_3_OR_NEWER
        targetId = project.GetUnityFrameworkTargetGuid();
#else
targetId = project.TargetGuidByName(PBXProject.GetUnityTargetName());
#endif
        return targetId;
    }

    public void OnPostGenerateGradleAndroidProject(string path)
    {
        Debug.Log("OnPostGenerateGradleAndroidProject "+path);
        string configPath = "Assets/esgconfig" + "/esg_config.json";
        string configContent = File.ReadAllText(configPath);
        JObject data = JObject.Parse(configContent);
        string fbId = (string)data.GetValue("fbId");
        string fbClientToken = (string)data.GetValue("fbClientToken");
        Debug.Log("fbId " + fbId + " fbClientToken " + fbClientToken);
        var androidManifest = new AndroidManifest(GetManifestPath(path));
        var metaList = androidManifest.ApplicationElement.GetElementsByTagName("meta-data");
        bool foundFbClientToken = false;

        bool foundFbAppId = false;
        foreach (XmlElement meta in metaList)
        {
            if (meta.GetAttribute("android:name").Equals("com.facebook.sdk.ApplicationId"))
            {
                Debug.Log("found fb id "+ meta.GetAttribute("com.facebook.sdk.ApplicationId"));
                meta.SetAttribute("replace", "http://schemas.android.com/tools","android:value");
                meta.SetAttribute("value", "http://schemas.android.com/apk/res/android", "fb"+fbId);
                foundFbAppId = true;
            }
            if (meta.GetAttribute("android:name").Equals("com.facebook.sdk.ClientToken"))
            {
                Debug.Log("found client token "+meta.GetAttribute("com.facebook.sdk.ClientToken"));
                meta.SetAttribute("replace", "http://schemas.android.com/tools", "android:value");
                meta.SetAttribute("value", "http://schemas.android.com/apk/res/android", fbClientToken);
                foundFbClientToken = true;
            }

        }
        Debug.Log("foundFbClientToken " + foundFbClientToken);
        if (!foundFbClientToken)
        {
            XmlElement meta = new AndroidXmlElement("", "meta-data","",androidManifest) ;
            meta.SetAttribute("name", "http://schemas.android.com/apk/res/android", "com.facebook.sdk.ClientToken");
            meta.SetAttribute("replace", "http://schemas.android.com/tools", "android:value");
            meta.SetAttribute("value", "http://schemas.android.com/apk/res/android", fbClientToken);
            androidManifest.ApplicationElement.AppendChild(meta);
        }
        if (!foundFbAppId)
        {
            XmlElement meta = new AndroidXmlElement("", "com.facebook.sdk.ApplicationId", "", androidManifest);
            meta.SetAttribute("name", "http://schemas.android.com/apk/res/android", "com.facebook.sdk.ApplicationId");
            meta.SetAttribute("replace", "http://schemas.android.com/tools", "android:value");
            meta.SetAttribute("value", "http://schemas.android.com/apk/res/android", "fb" + fbId);
            androidManifest.ApplicationElement.AppendChild(meta);
        }
        androidManifest.Save();
        string gpJsonPath = "Assets/Plugins/Android" + "/google-services.json";
        var content = File.ReadAllText(gpJsonPath);
        System.IO.File.WriteAllText(getGGServiceAndroidPath(path), content);
    }

    public int callbackOrder { get { return 1; } }

    private string _manifestFilePath;

    private string GetManifestPath(string basePath)
    {
        if (string.IsNullOrEmpty(_manifestFilePath))
        {
            var pathBuilder = new StringBuilder(basePath);
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("src");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("main");
            pathBuilder.Append(Path.DirectorySeparatorChar).Append("AndroidManifest.xml");
            _manifestFilePath = pathBuilder.ToString();
        }
        return _manifestFilePath;
    }

    internal class AndroidXmlDocument : XmlDocument
    {
        private string m_Path;
        protected XmlNamespaceManager nsMgr;
        public readonly string AndroidXmlNamespace = "http://schemas.android.com/apk/res/android";
        public AndroidXmlDocument(string path)
        {
            m_Path = path;
            using (var reader = new XmlTextReader(m_Path))
            {
                reader.Read();
                Load(reader);
            }
            nsMgr = new XmlNamespaceManager(NameTable);
            nsMgr.AddNamespace("android", AndroidXmlNamespace);
            nsMgr.AddNamespace("tools", "http://schemas.android.com/tools");
        }

        public string Save()
        {
            return SaveAs(m_Path);
        }

        public string SaveAs(string path)
        {
            using (var writer = new XmlTextWriter(path, new UTF8Encoding(false)))
            {
                writer.Formatting = Formatting.Indented;
                Save(writer);
            }
            return path;
        }
    }


    internal class AndroidManifest : AndroidXmlDocument
    {
        public readonly XmlElement ApplicationElement;

        public AndroidManifest(string path) : base(path)
        {
            ApplicationElement = SelectSingleNode("/manifest/application") as XmlElement;
            ApplicationElement.GetElementsByTagName("meta-data");
        }

        private XmlAttribute CreateAndroidAttribute(string key, string value)
        {
            XmlAttribute attr = CreateAttribute("android", key, AndroidXmlNamespace);
            attr.Value = value;
            return attr;
        }

        internal XmlNode GetActivityWithLaunchIntent()
        {
            return SelectSingleNode("/manifest/application/activity[intent-filter/action/@android:name='android.intent.action.MAIN' and " +
                    "intent-filter/category/@android:name='android.intent.category.LAUNCHER']", nsMgr);
        }

        internal void SetApplicationTheme(string appTheme)
        {
            ApplicationElement.Attributes.Append(CreateAndroidAttribute("theme", appTheme));
        }

        internal void SetStartingActivityName(string activityName)
        {
            GetActivityWithLaunchIntent().Attributes.Append(CreateAndroidAttribute("name", activityName));
        }
    }
    internal class AndroidXmlElement : XmlElement
    {

        public AndroidXmlElement(string prefix, string localName, string namespaceURI, XmlDocument doc) : base(prefix, localName, namespaceURI, doc)
        {
           
        }

    }
}
