// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen.exe, version 0.9.0.0
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Energistics.Etp.v12.Protocol.ChannelSubscribe
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class GetChannelMetadataResponse : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"GetChannelMetadataResponse\",\"namespace\":\"Energistics.Etp" +
				".v12.Protocol.ChannelSubscribe\",\"fields\":[{\"name\":\"metadata\",\"default\":[],\"type\"" +
				":{\"type\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"ChannelMetadataRecord\",\"namesp" +
				"ace\":\"Energistics.Etp.v12.Datatypes.ChannelData\",\"fields\":[{\"name\":\"uri\",\"type\":" +
				"\"string\"},{\"name\":\"id\",\"type\":\"long\"},{\"name\":\"indexes\",\"default\":[],\"type\":{\"ty" +
				"pe\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"IndexMetadataRecord\",\"namespace\":\"E" +
				"nergistics.Etp.v12.Datatypes.ChannelData\",\"fields\":[{\"name\":\"indexKind\",\"default" +
				"\":\"Time\",\"type\":{\"type\":\"enum\",\"name\":\"ChannelIndexKind\",\"namespace\":\"Energistic" +
				"s.Etp.v12.Datatypes.ChannelData\",\"symbols\":[\"Time\",\"Depth\"],\"fullName\":\"Energist" +
				"ics.Etp.v12.Datatypes.ChannelData.ChannelIndexKind\",\"depends\":[]}},{\"name\":\"inte" +
				"rval\",\"type\":{\"type\":\"record\",\"name\":\"IndexInterval\",\"namespace\":\"Energistics.Et" +
				"p.v12.Datatypes.Object\",\"fields\":[{\"name\":\"startIndex\",\"type\":{\"type\":\"record\",\"" +
				"name\":\"IndexValue\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\"" +
				":\"item\",\"type\":[\"null\",\"long\",\"double\"]}],\"fullName\":\"Energistics.Etp.v12.Dataty" +
				"pes.IndexValue\",\"depends\":[]}},{\"name\":\"endIndex\",\"type\":\"Energistics.Etp.v12.Da" +
				"tatypes.IndexValue\"},{\"name\":\"uom\",\"type\":\"string\"},{\"name\":\"depthDatum\",\"defaul" +
				"t\":\"\",\"type\":\"string\"}],\"fullName\":\"Energistics.Etp.v12.Datatypes.Object.IndexIn" +
				"terval\",\"depends\":[\r\n  \"Energistics.Etp.v12.Datatypes.IndexValue\",\r\n  \"Energisti" +
				"cs.Etp.v12.Datatypes.IndexValue\"\r\n]}},{\"name\":\"direction\",\"default\":\"Increasing\"" +
				",\"type\":{\"type\":\"enum\",\"name\":\"IndexDirection\",\"namespace\":\"Energistics.Etp.v12." +
				"Datatypes.ChannelData\",\"symbols\":[\"Increasing\",\"Decreasing\"],\"fullName\":\"Energis" +
				"tics.Etp.v12.Datatypes.ChannelData.IndexDirection\",\"depends\":[]}},{\"name\":\"name\"" +
				",\"default\":\"\",\"type\":\"string\"}],\"fullName\":\"Energistics.Etp.v12.Datatypes.Channe" +
				"lData.IndexMetadataRecord\",\"depends\":[\r\n  \"Energistics.Etp.v12.Datatypes.Channel" +
				"Data.ChannelIndexKind\",\r\n  \"Energistics.Etp.v12.Datatypes.Object.IndexInterval\"," +
				"\r\n  \"Energistics.Etp.v12.Datatypes.ChannelData.IndexDirection\"\r\n]}}},{\"name\":\"ch" +
				"annelName\",\"type\":\"string\"},{\"name\":\"dataType\",\"type\":\"string\"},{\"name\":\"uom\",\"t" +
				"ype\":\"string\"},{\"name\":\"measureClass\",\"type\":\"string\"},{\"name\":\"status\",\"type\":{" +
				"\"type\":\"enum\",\"name\":\"ChannelStatusKind\",\"namespace\":\"Energistics.Etp.v12.Dataty" +
				"pes.ChannelData\",\"symbols\":[\"Active\",\"Inactive\",\"Closed\"],\"fullName\":\"Energistic" +
				"s.Etp.v12.Datatypes.ChannelData.ChannelStatusKind\",\"depends\":[]}},{\"name\":\"sourc" +
				"e\",\"type\":\"string\"},{\"name\":\"axisVectorLengths\",\"type\":{\"type\":\"array\",\"items\":\"" +
				"int\"}},{\"name\":\"customData\",\"type\":{\"type\":\"map\",\"values\":{\"type\":\"record\",\"name" +
				"\":\"DataValue\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"ite" +
				"m\",\"type\":[\"null\",\"boolean\",\"int\",\"long\",\"float\",\"double\",\"string\",{\"type\":\"reco" +
				"rd\",\"name\":\"ArrayOfBoolean\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fields\"" +
				":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items\":\"boolean\"}}],\"fullName\":\"Energ" +
				"istics.Etp.v12.Datatypes.ArrayOfBoolean\",\"depends\":[]},{\"type\":\"record\",\"name\":\"" +
				"ArrayOfInt\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"value" +
				"s\",\"type\":{\"type\":\"array\",\"items\":\"int\"}}],\"fullName\":\"Energistics.Etp.v12.Datat" +
				"ypes.ArrayOfInt\",\"depends\":[]},{\"type\":\"record\",\"name\":\"ArrayOfLong\",\"namespace\"" +
				":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"arra" +
				"y\",\"items\":\"long\"}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.ArrayOfLong\",\"dep" +
				"ends\":[]},{\"type\":\"record\",\"name\":\"ArrayOfFloat\",\"namespace\":\"Energistics.Etp.v1" +
				"2.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items\":\"float\"}}" +
				"],\"fullName\":\"Energistics.Etp.v12.Datatypes.ArrayOfFloat\",\"depends\":[]},{\"type\":" +
				"\"record\",\"name\":\"ArrayOfDouble\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fie" +
				"lds\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items\":\"double\"}}],\"fullName\":\"En" +
				"ergistics.Etp.v12.Datatypes.ArrayOfDouble\",\"depends\":[]},{\"type\":\"record\",\"name\"" +
				":\"ArrayOfString\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"" +
				"values\",\"type\":{\"type\":\"array\",\"items\":\"string\"}}],\"fullName\":\"Energistics.Etp.v" +
				"12.Datatypes.ArrayOfString\",\"depends\":[]},\"bytes\"]}],\"fullName\":\"Energistics.Etp" +
				".v12.Datatypes.DataValue\",\"depends\":[\r\n  \"Energistics.Etp.v12.Datatypes.ArrayOfB" +
				"oolean\",\r\n  \"Energistics.Etp.v12.Datatypes.ArrayOfInt\",\r\n  \"Energistics.Etp.v12." +
				"Datatypes.ArrayOfLong\",\r\n  \"Energistics.Etp.v12.Datatypes.ArrayOfFloat\",\r\n  \"Ene" +
				"rgistics.Etp.v12.Datatypes.ArrayOfDouble\",\r\n  \"Energistics.Etp.v12.Datatypes.Arr" +
				"ayOfString\"\r\n]}}},{\"name\":\"attributeMetadata\",\"default\":[],\"type\":{\"type\":\"array" +
				"\",\"items\":{\"type\":\"record\",\"name\":\"AttributeMetadataRecord\",\"namespace\":\"Energis" +
				"tics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"attributeId\",\"type\":\"int\"},{\"name\":\"a" +
				"ttributeName\",\"type\":\"string\"},{\"name\":\"dataType\",\"type\":\"string\"},{\"name\":\"desc" +
				"ription\",\"type\":\"string\"}],\"fullName\":\"Energistics.Etp.v12.Datatypes.AttributeMe" +
				"tadataRecord\",\"depends\":[]}}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.Channel" +
				"Data.ChannelMetadataRecord\",\"depends\":[\r\n  \"Energistics.Etp.v12.Datatypes.Channe" +
				"lData.IndexMetadataRecord\",\r\n  \"Energistics.Etp.v12.Datatypes.ChannelData.Channe" +
				"lStatusKind\",\r\n  \"Energistics.Etp.v12.Datatypes.DataValue\",\r\n  \"Energistics.Etp." +
				"v12.Datatypes.AttributeMetadataRecord\"\r\n]}}},{\"name\":\"errors\",\"default\":[],\"type" +
				"\":{\"type\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"ErrorInfo\",\"namespace\":\"Energ" +
				"istics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"uri\",\"type\":\"string\"},{\"name\":\"mess" +
				"age\",\"type\":\"string\"},{\"name\":\"code\",\"type\":\"int\"}],\"fullName\":\"Energistics.Etp." +
				"v12.Datatypes.ErrorInfo\",\"depends\":[]}}}],\"protocol\":\"21\",\"messageType\":\"2\",\"sen" +
				"derRole\":\"producer\",\"protocolRoles\":\"producer,consumer\",\"multipartFlag\":true,\"fu" +
				"llName\":\"Energistics.Etp.v12.Protocol.ChannelSubscribe.GetChannelMetadataRespons" +
				"e\",\"depends\":[\r\n  \"Energistics.Etp.v12.Datatypes.ChannelData.ChannelMetadataReco" +
				"rd\",\r\n  \"Energistics.Etp.v12.Datatypes.ErrorInfo\"\r\n]}");
		private IList<Energistics.Etp.v12.Datatypes.ChannelData.ChannelMetadataRecord> _metadata;
		private IList<Energistics.Etp.v12.Datatypes.ErrorInfo> _errors;
		public virtual Schema Schema
		{
			get
			{
				return GetChannelMetadataResponse._SCHEMA;
			}
		}
		public IList<Energistics.Etp.v12.Datatypes.ChannelData.ChannelMetadataRecord> Metadata
		{
			get
			{
				return this._metadata;
			}
			set
			{
				this._metadata = value;
			}
		}
		public IList<Energistics.Etp.v12.Datatypes.ErrorInfo> Errors
		{
			get
			{
				return this._errors;
			}
			set
			{
				this._errors = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this._metadata;
			case 1: return this._errors;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this._metadata = (IList<Energistics.Etp.v12.Datatypes.ChannelData.ChannelMetadataRecord>)fieldValue; break;
			case 1: this._errors = (IList<Energistics.Etp.v12.Datatypes.ErrorInfo>)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}