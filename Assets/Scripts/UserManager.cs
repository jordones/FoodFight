using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class UserManager : MonoBehaviour {

	public static UserManager instance = null;     
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Database.DatabaseReference db;
    public bool ready = false;

    // Handle initialization of the necessary firebase modules:
    public void InitializeFirebase() {
        Debug.Log("Setting up Firebase");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cis4250-rot.firebaseio.com/");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        db = FirebaseDatabase.DefaultInstance.GetReference("users");

        AuthStateChanged(this, null);
    }

	public void Login(string email, string password) {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });

	}
    // Track state changes of the auth object.
    private void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    private int getInt(string path, string item) {
        return db.Child(path).Child(item).GetValueAsync().ContinueWith(task => {
            if (task.IsFaulted) {
                return 0; //FIXME
            }
            else if (task.IsCompleted) {
                DataSnapshot snapshot = task.Result;
                return snapshot.Value;
            }
        });
    }

    private void setInt(string item, int value) {
        db.Child(user.UserId).Child(item).SetValueAsync(value);
    }

    private int _deaths;
    private int _runs;
    private List<int> _items;
    public int deaths {
        get {
            bool signedIn = auth.CurrentUser != null;
            if (signedIn) {
                _deaths = getInt(user.UserId, "deaths");
            }
            return _deaths;
        }
        set {
            _deaths = value;
            bool signedIn = auth.CurrentUser != null;
            if (signedIn) {
                setInt("deaths", value);
            }
        }
    }
    public int runs {
        get {
            return _runs;
        }
        set {
            _runs = value;
        }
    }
    public List<int> items {
        get {
            return _items;
        }
        set {
            _items = value;
        }
    }
	// Use this for initialization
	void Start () {
	}

	void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);

            string email = "buckleywdavid@gmail.com";
            string password = "abc123";

            InitializeFirebase();
            Login(email, password);
            ready = true;
        } else {
            Destroy(gameObject);
        }
	}

    void OnGUI() {
        if (user != null) {
            GUI.Label(new Rect(10, 10, 200, 30), "" + instance);
        } else {
            GUI.Label(new Rect(10, 10, 100, 30), "Logged in: None");
        }

    }

	void Update () {
		
	}

    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    public void Save() {
        string dataPath = Application.persistentDataPath + "/UserInfo.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);

        UserInfo data = new UserInfo();
        data.deaths = deaths;
        data.runs = runs;
        data.items = items;

        bf.Serialize(file, data);
    }

    public void Load() {
        string dataPath = Application.persistentDataPath + "/UserInfo.dat";
        if (File.Exists(dataPath)) {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(dataPath, FileMode.Open);
            UserInfo data = (UserInfo)bf.Deserialize(file);

            deaths = data.deaths;
            runs = data.runs;
            items = data.items;
        }
    }
}

[Serializable]
class UserInfo {
    public int runs;
    public int deaths;
    public List<int> items;
}