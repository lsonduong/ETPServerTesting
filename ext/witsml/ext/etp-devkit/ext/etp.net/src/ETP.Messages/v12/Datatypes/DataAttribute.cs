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
	
	public partial class DataAttribute : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"DataAttribute\",\"namespace\":\"Energistics.Etp.v12.Datatype" +
				"s\",\"fields\":[{\"name\":\"attributeId\",\"type\":\"int\"},{\"name\":\"attributeValue\",\"type\"" +
				":{\"type\":\"record\",\"name\":\"DataValue\",\"namespace\":\"Energistics.Etp.v12.Datatypes\"" +
				",\"fields\":[{\"name\":\"item\",\"type\":[\"null\",\"boolean\",\"int\",\"long\",\"float\",\"double\"" +
				",\"string\",{\"type\":\"record\",\"name\":\"ArrayOfBoolean\",\"namespace\":\"Energistics.Etp." +
				"v12.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items\":\"boolea" +
				"n\"}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.ArrayOfBoolean\",\"depends\":[]},{\"" +
				"type\":\"record\",\"name\":\"ArrayOfInt\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"" +
				"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items\":\"int\"}}],\"fullName\":\"En" +
				"ergistics.Etp.v12.Datatypes.ArrayOfInt\",\"depends\":[]},{\"type\":\"record\",\"name\":\"A" +
				"rrayOfLong\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"value" +
				"s\",\"type\":{\"type\":\"array\",\"items\":\"long\"}}],\"fullName\":\"Energistics.Etp.v12.Data" +
				"types.ArrayOfLong\",\"depends\":[]},{\"type\":\"record\",\"name\":\"ArrayOfFloat\",\"namespa" +
				"ce\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"a" +
				"rray\",\"items\":\"float\"}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.ArrayOfFloat\"" +
				",\"depends\":[]},{\"type\":\"record\",\"name\":\"ArrayOfDouble\",\"namespace\":\"Energistics." +
				"Etp.v12.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items\":\"do" +
				"uble\"}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.ArrayOfDouble\",\"depends\":[]}," +
				"{\"type\":\"record\",\"name\":\"ArrayOfString\",\"namespace\":\"Energistics.Etp.v12.Datatyp" +
				"es\",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items\":\"string\"}}],\"fullN" +
				"ame\":\"Energistics.Etp.v12.Datatypes.ArrayOfString\",\"depends\":[]},\"bytes\"]}],\"ful" +
				"lName\":\"Energistics.Etp.v12.Datatypes.DataValue\",\"depends\":[\r\n  \"Energistics.Etp" +
				".v12.Datatypes.ArrayOfBoolean\",\r\n  \"Energistics.Etp.v12.Datatypes.ArrayOfInt\",\r\n" +
				"  \"Energistics.Etp.v12.Datatypes.ArrayOfLong\",\r\n  \"Energistics.Etp.v12.Datatypes" +
				".ArrayOfFloat\",\r\n  \"Energistics.Etp.v12.Datatypes.ArrayOfDouble\",\r\n  \"Energistic" +
				"s.Etp.v12.Datatypes.ArrayOfString\"\r\n]}}],\"fullName\":\"Energistics.Etp.v12.Datatyp" +
				"es.DataAttribute\",\"depends\":[\r\n  \"Energistics.Etp.v12.Datatypes.DataValue\"\r\n]}");
		private int _attributeId;
		private Energistics.Etp.v12.Datatypes.DataValue _attributeValue;
		public virtual Schema Schema
		{
			get
			{
				return DataAttribute._SCHEMA;
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
		public Energistics.Etp.v12.Datatypes.DataValue AttributeValue
		{
			get
			{
				return this._attributeValue;
			}
			set
			{
				this._attributeValue = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this._attributeId;
			case 1: return this._attributeValue;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this._attributeId = (System.Int32)fieldValue; break;
			case 1: this._attributeValue = (Energistics.Etp.v12.Datatypes.DataValue)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
