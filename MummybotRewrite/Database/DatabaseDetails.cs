using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


public class DatabaseDetails
{
    public string TokenDB { get; set; } = "";
    public string RuntimeDB { get; set; } = "";

    public string RuntimeDebugDB { get; set; } = "";


    internal void SaveDetials()
    => File.WriteAllText(@"database.json", JsonConvert.SerializeObject(this));

    internal DatabaseDetails LoadDetials()
        => JsonConvert.DeserializeObject<DatabaseDetails>(File.ReadAllText(@"database.json"));
    


    public string GetRuntimeDB()
    {
#if DEBUG
        return RuntimeDebugDB;

#else
        return RuntimeDB;
#endif
    }
    public string GetTokenDB()
    {
        return TokenDB;

    }

}

