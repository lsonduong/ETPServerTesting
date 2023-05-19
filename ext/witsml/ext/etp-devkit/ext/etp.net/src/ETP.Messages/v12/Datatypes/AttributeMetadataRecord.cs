// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen.exe, version 0.9.0.0
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Energistics.Etp.v12.Datatypes
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class AttributeMetadataRecord : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse(@"{""type"":""record"",""name"":""AttributeMetadataRecord"",""namespace"":""Energistics.Etp.v12.Datatypes"",""fields"":[{""name"":""attributeId"",""type"":""int""},{""name"":""attributeName"",""type"":""string""},{""name"":""dataType"",""type"":""string""},{""name"":""description"",""type"":""string""}],""fullName"":""Energistics.Etp.v12.Datatypes.AttributeMetadataRecord"",""depends"":[]}");
		private int _attributeId;
		private string _attributeName;
		private string _dataType;
		private string _description;
		public virtual Schema Schema
		{
			get
			{
				return AttributeMetadataRecord._SCHEMA;
			}
		}
		public int AttributeId
		{
			get
			{
				return this._attributeId;
			}
			set
			{
				this._attributeId = value;
			}
		}
		public string AttributeName
		{
			get
			{
				return this._attributeName;
			}
			set
			{
				this._attributeName = value;
			}
		}
		public string DataType
		{
			get
			{
				return this._dataType;
			}
			set
			{
				this._dataType = value;
			}
		}
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				this._description = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this._attributeId;
			case 1: return this._attributeName;
			case 2: return this._dataType;
			case 3: return this._description;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this._attributeId = (System.Int32)fieldValue; break;
			case 1: this._attributeName = (System.String)fieldValue; break;
			case 2: this._dataType = (System.String)fieldValue; break;
			case 3: this._description = (System.String)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
