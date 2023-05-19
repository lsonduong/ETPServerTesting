// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen.exe, version 0.9.0.0
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Energistics.Etp.v12.Protocol.GrowingObjectNotification
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class PartChanged : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse(@"{""type"":""record"",""name"":""PartChanged"",""namespace"":""Energistics.Etp.v12.Protocol.GrowingObjectNotification"",""fields"":[{""name"":""changeKind"",""type"":{""type"":""enum"",""name"":""ObjectChangeKind"",""namespace"":""Energistics.Etp.v12.Datatypes.Object"",""symbols"":[""insert"",""update"",""authorized""],""fullName"":""Energistics.Etp.v12.Datatypes.Object.ObjectChangeKind"",""depends"":[]}},{""name"":""changeTime"",""type"":""long""},{""name"":""uri"",""type"":""string""},{""name"":""contentType"",""type"":""string""},{""name"":""uid"",""type"":""string""},{""name"":""data"",""type"":""bytes""}],""protocol"":""7"",""messageType"":""2"",""senderRole"":""store"",""protocolRoles"":""store,customer"",""multipartFlag"":false,""fullName"":""Energistics.Etp.v12.Protocol.GrowingObjectNotification.PartChanged"",""depends"":[
  ""Energistics.Etp.v12.Datatypes.Object.ObjectChangeKind""
]}");
		private Energistics.Etp.v12.Datatypes.Object.ObjectChangeKind _changeKind;
		private long _changeTime;
		private string _uri;
		private string _contentType;
		private string _uid;
		private byte[] _data;
		public virtual Schema Schema
		{
			get
			{
				return PartChanged._SCHEMA;
			}
		}
		public Energistics.Etp.v12.Datatypes.Object.ObjectChangeKind ChangeKind
		{
			get
			{
				return this._changeKind;
			}
			set
			{
				this._changeKind = value;
			}
		}
		public long ChangeTime
		{
			get
			{
				return this._changeTime;
			}
			set
			{
				this._changeTime = value;
			}
		}
		public string Uri
		{
			get
			{
				return this._uri;
			}
			set
			{
				this._uri = value;
			}
		}
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
			set
			{
				this._contentType = value;
			}
		}
		public string Uid
		{
			get
			{
				return this._uid;
			}
			set
			{
				this._uid = value;
			}
		}
		public byte[] Data
		{
			get
			{
				return this._data;
			}
			set
			{
				this._data = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this._changeKind;
			case 1: return this._changeTime;
			case 2: return this._uri;
			case 3: return this._contentType;
			case 4: return this._uid;
			case 5: return this._data;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this._changeKind = (Energistics.Etp.v12.Datatypes.Object.ObjectChangeKind)fieldValue; break;
			case 1: this._changeTime = (System.Int64)fieldValue; break;
			case 2: this._uri = (System.String)fieldValue; break;
			case 3: this._contentType = (System.String)fieldValue; break;
			case 4: this._uid = (System.String)fieldValue; break;
			case 5: this._data = (System.Byte[])fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}