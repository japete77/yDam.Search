using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using yDevs.Shared.Serializers;

namespace yDam.Dam.Model.Assets
{
    public class Asset : INotifyPropertyChanged
    {        
        private readonly decimal DefaultPermission = 484; // Default permission is rwxr--r--
        private ObjectId _id;
        private string _id_;
        private string _owner;
        private string _group;
        private decimal _permissions;
        private DateTime? _last_update_date;
        private string _last_update_user;
        private asset_type _asset_type;
        private ObservableCollection<string> _parent_folder;
        private object _model;

        public Asset()
        {
            this._permissions = DefaultPermission;
        }

        [BsonId]
        [JsonProperty("id", Required = Required.Always)]
        public ObjectId Id
        {
            get { return _id; }
            set 
            {
                if (_id != value)
                {
                    _id = value;
                    _id_ = value.ToString();
                    RaisePropertyChanged();
                }
            }
        }

        [JsonProperty("id_", Required = Required.Default)]
        public string Id_
        {
            get { return _id_; }
        }

        [BsonElement("owner")]
        [JsonProperty("owner", Required = Required.Always)]
        public string Owner
        {
            get { return _owner; }
            set 
            {
                if (_owner != value)
                {
                    _owner = value; 
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("group")]
        [JsonProperty("group", Required = Required.Always)]
        public string Group
        {
            get { return _group; }
            set 
            {
                if (_group != value)
                {
                    _group = value; 
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("permissions")]
        [JsonProperty("permissions", Required = Required.Default)]
        public decimal Permissions
        {
            get { return _permissions; }
            set 
            {
                if (_permissions != value)
                {
                    _permissions = value; 
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("last_update_date")]
        [JsonProperty("last_update_date", Required = Required.Default)]
        public DateTime? LastUpdateDate
        {
            get { return _last_update_date; }
            set 
            {
                if (_last_update_date != value)
                {
                    _last_update_date = value; 
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("last_update_user")]
        [JsonProperty("last_update_user", Required = Required.Default)]
        public string LastUpdateUser
        {
            get { return _last_update_user; }
            set 
            {
                if (_last_update_user != value)
                {
                    _last_update_user = value; 
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("asset_type")]
        [JsonProperty("asset_type", Required = Required.Always)]
        [JsonConverter(typeof(StringEnumConverter))]
        public asset_type AssetType
        {
            get { return _asset_type; }
            set 
            {
                if (_asset_type != value)
                {
                    _asset_type = value; 
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("parent_folder")]
        [JsonProperty("parent_folder", Required = Required.Default)]
        public ObservableCollection<string> ParentFolder
        {
            get { return _parent_folder; }
            set 
            {
                if (_parent_folder != value)
                {
                    _parent_folder = value;
                    RaisePropertyChanged();
                }
            }
        }

        [BsonElement("model")]        
        [JsonProperty("model", Required = Required.Default)]
        [BsonSerializer(typeof(CustomObjectSerializer))]
        public object Model 
        { 
            get { return _model; } 
            set 
            { 
                if (_model != value)
                {
                    _model = value;
                    RaisePropertyChanged();
                }
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string ToJson() 
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Asset FromJson(string data)
        {
            return JsonConvert.DeserializeObject<Asset>(data);
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) 
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum asset_type
    {
        Audio = 0, 
        Document = 1, 
        Folder = 2, 
        Video = 3, 
        Image = 4, 
    }
}