// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen.exe, version 0.9.0.0
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Energistics.Etp.v12.Protocol.ChannelDataFrame
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class ChannelDataFrameSet : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"ChannelDataFrameSet\",\"namespace\":\"Energistics.Etp.v12.Pr" +
				"otocol.ChannelDataFrame\",\"fields\":[{\"name\":\"channels\",\"type\":{\"type\":\"array\",\"it" +
				"ems\":\"long\"}},{\"name\":\"data\",\"type\":{\"type\":\"array\",\"items\":{\"type\":\"record\",\"na" +
				"me\":\"DataFrame\",\"namespace\":\"Energistics.Etp.v12.Datatypes.ChannelData\",\"fields\"" +
				":[{\"name\":\"indexes\",\"type\":{\"type\":\"array\",\"items\":\"long\"}},{\"name\":\"data\",\"type" +
				"\":{\"type\":\"array\",\"items\":{\"type\":\"record\",\"name\":\"DataValue\",\"namespace\":\"Energ" +
				"istics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"item\",\"type\":[\"null\",\"boolean\",\"int" +
				"\",\"long\",\"float\",\"double\",\"string\",{\"type\":\"record\",\"name\":\"ArrayOfBoolean\",\"nam" +
				"espace\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"type" +
				"\":\"array\",\"items\":\"boolean\"}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.ArrayOf" +
				"Boolean\",\"depends\":[]},{\"type\":\"record\",\"name\":\"ArrayOfInt\",\"namespace\":\"Energis" +
				"tics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items" +
				"\":\"int\"}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.ArrayOfInt\",\"depends\":[]},{" +
				"\"type\":\"record\",\"name\":\"ArrayOfLong\",\"namespace\":\"Energistics.Etp.v12.Datatypes\"" +
				",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"items\":\"long\"}}],\"fullName\":" +
				"\"Energistics.Etp.v12.Datatypes.ArrayOfLong\",\"depends\":[]},{\"type\":\"record\",\"name" +
				"\":\"ArrayOfFloat\",\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"" +
				"values\",\"type\":{\"type\":\"array\",\"items\":\"float\"}}],\"fullName\":\"Energistics.Etp.v1" +
				"2.Datatypes.ArrayOfFloat\",\"depends\":[]},{\"type\":\"record\",\"name\":\"ArrayOfDouble\"," +
				"\"namespace\":\"Energistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"" +
				"type\":\"array\",\"items\":\"double\"}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.Arra" +
				"yOfDouble\",\"depends\":[]},{\"type\":\"record\",\"name\":\"ArrayOfString\",\"namespace\":\"En" +
				"ergistics.Etp.v12.Datatypes\",\"fields\":[{\"name\":\"values\",\"type\":{\"type\":\"array\",\"" +
				"items\":\"string\"}}],\"fullName\":\"Energistics.Etp.v12.Datatypes.ArrayOfString\",\"dep" +
				"ends\":[]},\"bytes\"]}],\"fullName\":\"Energistics.Etp.v12.Datatypes.DataValue\",\"depen" +
				"ds\":[\r\n  \"Energistics.Etp.v12.Datatypes.ArrayOfBoolean\",\r\n  \"Energistics.Etp.v12" +
				".Datatypes.ArrayOfInt\",\r\n  \"Energistics.Etp.v12.Datatypes.ArrayOfLong\",\r\n  \"Ener" +
				"gistics.Etp.v12.Datatypes.ArrayOfFloat\",\r\n  \"Energistics.Etp.v12.Datatypes.Array" +
				"OfDouble\",\r\n  \"Energistics.Etp.v12.Datatypes.ArrayOfString\"\r\n]}}}],\"fullName\":\"E" +
				"nergistics.Etp.v12.Datatypes.ChannelData.DataFrame\",\"depends\":[\r\n  \"Energistics." +
				"Etp.v12.Datatypes.DataValue\"\r\n]}}}],\"protocol\":\"2\",\"messageType\":\"4\",\"senderRole" +
				"\":\"producer\",\"protocolRoles\":\"producer,consumer\",\"multipartFlag\":false,\"fullName" +
				"\":\"Energistics.Etp.v12.Protocol.ChannelDataFrame.ChannelDataFrameSet\",\"depends\":" +
				"[\r\n  \"Energistics.Etp.v12.Datatypes.ChannelData.DataFrame\"\r\n]}");
		private IList<System.Int64> _channels;
		private IList<Energistics.Etp.v12.Datatypes.ChannelData.DataFrame> _data;
		public virtual Schema Schema
		{
			get
			{
				return ChannelDataFrameSet._SCHEMA;
			}
		}
		public IList<System.Int64> Channels
		{
			get
			{
				return this._channels;
			}
			set
			{
				this._channels = value;
			}
		}
		public IList<Energistics.Etp.v12.Datatypes.ChannelData.DataFrame> Data
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
			case 0: return this._channels;
			case 1: return this._data;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this._channels = (IList<System.Int64>)fieldValue; break;
			case 1: this._data = (IList<Energistics.Etp.v12.Datatypes.ChannelData.DataFrame>)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}