using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

#nullable disable
namespace Essence.Elements
{
    public class Owner
    {
        public string username;
        public string profilePicture;
        public string role;
        public string lastActive;
        public DateTime createdAt;
        public bool verified;
        public bool isBanned;

        public void Correct()
        {
            if (!this.profilePicture.StartsWith("/images/"))
                return;
            this.profilePicture = "https://scriptblox.com" + this.profilePicture;
        }
    }
    public class Game
    {
        public string gameId;
        public string name;
        public string imageUrl;

        public void Correct()
        {
            if (!this.imageUrl.StartsWith("/images/"))
                return;
            this.imageUrl = "https://scriptblox.com" + this.imageUrl;
        }
    }
    public class ScriptObject
    {
        public string title;
        public string _id;
        public string features;
        public string script;
        public string slug;
        public DateTime createdAt;
        public DateTime updatedAt;
        public bool verified;
        public bool key;
        public bool isUniversal;
        public bool isPatched;
        public int views;
        public int likeCount;
        public int dislikeCount;
        public Game game;
        public Owner owner;
        public bool saved = false;

        public void Correct()
        {
            this.game?.Correct();
            this.owner?.Correct();
        }

        public async Task GetDetailedObject()
        {
            ScriptObject scriptObject = this;
            using (HttpClient client = new HttpClient())
            {
                foreach (JProperty jproperty in JToken.Parse(await (await client.GetAsync("https://scriptblox.com/api/script/" + scriptObject.slug)).Content.ReadAsStringAsync())[(object)"script"].Cast<JProperty>())
                {
                    FieldInfo field = scriptObject.GetType().GetField(jproperty.Name);
                    field?.SetValue((object)scriptObject, jproperty.Value.ToObject(field.FieldType));
                }
            }
        }

        public async Task<string> GetScript()
        {
            if (this.script == null)
                await this.GetDetailedObject();
            return this.script;
        }
    }
}
