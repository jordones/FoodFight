using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Threading.Tasks;

using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class UserManager : MonoBehaviour {


	public static UserManager instance = null;     
    private Firebase.Auth.FirebaseAuth auth;
    private Firebase.Auth.FirebaseUser user;
    private Firebase.Database.DatabaseReference db;
    public bool ready = false;

    public int deaths {get; set;} = 0;
    public int runs {get; set;} = 0;
    public List<int> items {get; set;} = new List<int>(){0};

    // Handle initialization of the necessary firebase modules:
    public void InitializeFirebase() {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;

        Debug.Log("Setting up Firebase");
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://cis4250-rot.firebaseio.com/");
        db = FirebaseDatabase.DefaultInstance.GetReference("/users");
    }

    public bool loggedIn() {
        return user != null;
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

    public void SignUp(string email, string password) {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            // Firebase user has been created.
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void Logout() {
        if (auth != null)
            auth.SignOut();
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

    private T getFirebaseValue<T>(string path, string item) {
        Task<T> t = db.Child(path).Child(item)
            .GetValueAsync()
            .ContinueWith(task => {
                if (task.IsFaulted) {
                    Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
                    return default(T);
                }
                else if (task.IsCompleted) {
                    DataSnapshot snapshot = task.Result;
                    return (T) snapshot.Value;
                }
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
                return default(T);
        });
        return t.Result;
    }

    private Task<Firebase.Database.DataSnapshot> getFirebaseAsync<T>(string path, string item) {
         return db.Child(path).Child(item).GetValueAsync();
    }

    private void setFirebaseScalar<T>(string path, string item, T value) {
        db.Child(path).Child(item).SetValueAsync(value);
    }

    private void setFirebaseList<T>(string path, string item, List<T> list) {
        // Gross manual json serialization
        string json = "[";
        foreach (var value in list) {
            json = json + value + ",";
        }
        json = json.Substring(0, json.Length-1) + "]";

        db.Child(path).Child(item).SetRawJsonValueAsync(json);
    }

	void Start () {
	}

	void Awake () {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeFirebase();
            LoadLocal();
            ready = true;
        } else {
            Destroy(gameObject);
        }
	}

    void OnGUI() {
        if (user != null) {
            GUI.Label(new Rect(10, 10, 200, 30), user.UserId);
        } else {
            GUI.Label(new Rect(10, 10, 100, 30), "Logged in: None");
        }
        GUI.Label(new Rect(10, 50, 200, 30), "Deaths: " + deaths);
        GUI.Label(new Rect(10, 100, 200, 30), "Runs: " + runs);
        GUI.Label(new Rect(10, 150, 200, 30), "Items: [" + String.Join(", ", items) + "]");
    }

	void Update () {
		
	}

    void OnDestroy() {
        if (auth != null)
            auth.StateChanged -= AuthStateChanged;

        if (object.ReferenceEquals(this, instance)) {
            SaveLocal();
        }

        auth = null;
    }

    public void SaveFirebase() {
        Debug.Log("Saving to Firebase");
        setFirebaseScalar<int>(user.UserId, "deaths", deaths);
        setFirebaseScalar<int>(user.UserId, "runs", runs);
        setFirebaseList(user.UserId, "items", items);
    }

    // public void LoadFirebase() {
    //     Debug.Log("Loading from Firebase");
    //     deaths = Convert.ToInt32(getFirebaseValue<long>(user.UserId, "deaths"));
    //     runs = Convert.ToInt32(getFirebaseValue<long>(user.UserId, "runs"));
    //     items = getFirebaseValue<List<long>>(user.UserId, "items").ConvertAll(i => Convert.ToInt32(i));
    // }

    public IEnumerator LoadFirebaseAsync() {
        Debug.Log("Loading from Firebase Async");
        Task<Firebase.Database.DataSnapshot> tDeaths = getFirebaseAsync<int>(user.UserId, "deaths");
        Task<Firebase.Database.DataSnapshot> tRuns = getFirebaseAsync<int>(user.UserId, "runs");
        Task<Firebase.Database.DataSnapshot> tItems = getFirebaseAsync<List<long>>(user.UserId, "items");

        tDeaths.ContinueWith(task => {
            Debug.Log("Loading from Firebase Async - deaths task: " + task.Status + "- " + task.IsCompleted);
            if (task.IsFaulted) {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted) {
                Debug.Log("Loading from Firebase Async - changing deaths, current: " + deaths);
                DataSnapshot snapshot = task.Result;
                // deaths = (int) snapshot.Value;
                deaths = Convert.ToInt32(snapshot.Value);
            }
        });

        tRuns.ContinueWith(task => {
            Debug.Log("Loading from Firebase Async - runs task: " + task.Status + "- " + task.IsCompleted);
            if (task.IsFaulted) {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted) {
                Debug.Log("Loading from Firebase Async - changing runs, current: " + runs);
                DataSnapshot snapshot = task.Result;
                runs = Convert.ToInt32(snapshot.Value);
            }
        });

        tItems.ContinueWith(task => {
            Debug.Log("Loading from Firebase Async - items task: " + task.Status + "- " + task.IsCompleted);
            if (task.IsFaulted) {
                Debug.LogError("GetValueAsync encountered an error: " + task.Exception);
            }
            else if (task.IsCompleted) {
                Debug.Log("Loading from Firebase Async - changing items, current: " + items);
                DataSnapshot snapshot = task.Result;
                items = ((List<object>)snapshot.GetValue(false)).ConvertAll(i => Convert.ToInt32(i));
            }
        });

        yield return new WaitUntil(() => tDeaths.IsCompleted && tRuns.IsCompleted && tItems.IsCompleted);
    }

    public void SaveLocal() {
        Debug.Log("Saving Locally");
        string dataPath = Application.persistentDataPath + "/UserInfo.dat";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(dataPath);

        UserInfo data = new UserInfo();
        data.deaths = deaths;
        data.runs = runs;
        data.items = items;

        bf.Serialize(file, data);
    }

    public void LoadLocal() {
        Debug.Log("Loading Locally");
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