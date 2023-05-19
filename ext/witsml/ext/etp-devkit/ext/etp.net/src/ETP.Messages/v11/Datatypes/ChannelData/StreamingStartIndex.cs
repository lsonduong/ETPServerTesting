// ------------------------------------------------------------------------------
// <auto-generated>
//    Generated by avrogen.exe, version 0.9.0.0
//    Changes to this file may cause incorrect behavior and will be lost if code
//    is regenerated
// </auto-generated>
// ------------------------------------------------------------------------------
namespace Energistics.Etp.v11.Datatypes.ChannelData
{
	using System;
	using System.Collections.Generic;
	using System.Text;
	using Avro;
	using Avro.Specific;
	
	public partial class StreamingStartIndex : ISpecificRecord
	{
		public static Schema _SCHEMA = Avro.Schema.Parse("{\"type\":\"record\",\"name\":\"StreamingStartIndex\",\"namespace\":\"Energistics.Datatypes." +
				"ChannelData\",\"fields\":[{\"name\":\"item\",\"type\":[\"null\",\"int\",\"long\"]}],\"fullName\":" +
				"\"Energistics.Datatypes.ChannelData.StreamingStartIndex\",\"depends\":[]}");
		private object _item;
		public virtual Schema Schema
		{
			get
			{
				return StreamingStartIndex._SCHEMA;
			}
		}
		public object Item
		{
			get
			{
				return this._item;
			}
			set
			{
				this._item = value;
			}
		}
		public virtual object Get(int fieldPos)
		{
			switch (fieldPos)
			{
			case 0: return this._item;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Get()");
			};
		}
		public virtual void Put(int fieldPos, object fieldValue)
		{
			switch (fieldPos)
			{
			case 0: this._item = (System.Object)fieldValue; break;
			default: throw new AvroRuntimeException("Bad index " + fieldPos + " in Put()");
			};
		}
	}
}
