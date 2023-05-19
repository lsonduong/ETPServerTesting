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
	
	public partial class PartsReplacedByRange : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse(@"{""type"":""record"",""name"":""PartsReplacedByRange"",""namespace"":""Energistics.Etp.v12.Protocol.GrowingObjectNotification"",""fields"":[{""name"":""uri"",""type"":""string""},{""name"":""deletedInterval"",""type"":{""type"":""record"",""name"":""IndexInterval"",""namespace"":""Energistics.Etp.v12.Datatypes.Object"",""fields"":[{""name"":""startIndex"",""type"":{""type"":""record"",""name"":""IndexValue"",""namespace"":""Energistics.Etp.v12.Datatypes"",""fields"":[{""name"":""item"",""type"":[""null"",""long"",""double""]}],""fullName"":""Energistics.Etp.v12.Datatypes.IndexValue"",""depends"":[]}},{""name"":""endIndex"",""type"":""Energistics.Etp.v12.Datatypes.IndexValue""},{""name"":""uom"",""type"":""string""},{""name"":""depthDatum"",""default"":"""",""type"":""string""}],""fullName"":""Energistics.Etp.v12.Datatypes.Object.IndexInterval"",""depends"":[
  ""Energistics.Etp.v12.Datatypes.IndexValue"",
  ""Energistics.Etp.v12.Datatypes.IndexValue""
]}},{""name"":""includeOverlappingIntervals"",""type"":""boolean""},{""name"":""contentType"",""type"":""string""},{""name"":""uid"",""type"":""string""},{""name"":""data"",""type"":""bytes""},{""name"":""changeTime"",""type"":""long""}],""protocol"":""7"",""messageType"":""6"",""senderRole"":""store"",""protocolRoles"":""store,customer"",""multipartFlag"":true,""fullName"":""Energistics.Etp.v12.Protocol.GrowingObjectNotification.PartsReplacedByRange"",""depends"":[
  ""Energistics.Etp.v12.Datatypes.Object.IndexInterval""
]}");
		private string _uri;
		private Energistics.Etp.v12.Datatypes.Object.IndexInterval _deletedInterval;
		private bool _includeOverlappingIntervals;
		private string _contentType;
		private string _uid;
		private byte[] _data;
		private long _changeTime;
		public virtual Schema Schema
		{
			get
			{
				return PartsReplacedByRange._SCHEMA;
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
		public Energistics.Etp.v12.Datatypes.Object.IndexInterval DeletedInterval
		{
			get
			{
				return this._deletedInterval;
			}
			set
			{
				this._deletedInterval = value;
			}
		}
		public bool IncludeOverlappingIntervals
		{
			get
			{
				return this._includeOverlappingIntervals;
			}
			set
			{
				this._includeOverlappingIntervals = value;
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
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this._uri;
			case 1: return this._deletedInterval;
			case 2: return this._includeOverlappingIntervals;
			case 3: return this._contentType;
			case 4: return this._uid;
			case 5: return this._data;
			case 6: return this._changeTime;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this._uri = (System.String)fieldValue; break;
			case 1: this._deletedInterval = (Energistics.Etp.v12.Datatypes.Object.IndexInterval)fieldValue; break;
			case 2: this._includeOverlappingIntervals = (System.Boolean)fieldValue; break;
			case 3: this._contentType = (System.String)fieldValue; break;
			case 4: this._uid = (System.String)fieldValue; break;
			case 5: this._data = (System.Byte[])fieldValue; break;
			case 6: this._changeTime = (System.Int64)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
