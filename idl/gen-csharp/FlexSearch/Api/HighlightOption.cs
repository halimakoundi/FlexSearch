/**
 * Autogenerated by Thrift Compiler (0.9.1)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.ServiceModel;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace FlexSearch.Api
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  [DataContract(Namespace="")]
  public partial class HighlightOption : TBase
  {
    private short _FragmentsToReturn;
    private string _PostTag;
    private string _PreTag;

    [DataMember(Order = 1)]
    public short FragmentsToReturn
    {
      get
      {
        return _FragmentsToReturn;
      }
      set
      {
        __isset.FragmentsToReturn = true;
        this._FragmentsToReturn = value;
      }
    }

    [DataMember(Order = 2)]
    public List<string> HighlightedFields { get; set; }

    [DataMember(Order = 3)]
    public string PostTag
    {
      get
      {
        return _PostTag;
      }
      set
      {
        __isset.PostTag = true;
        this._PostTag = value;
      }
    }

    [DataMember(Order = 4)]
    public string PreTag
    {
      get
      {
        return _PreTag;
      }
      set
      {
        __isset.PreTag = true;
        this._PreTag = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    [DataContract]
    public struct Isset {
      public bool FragmentsToReturn;
      public bool PostTag;
      public bool PreTag;
    }

    public HighlightOption() {
      this._FragmentsToReturn = 2;
      this.__isset.FragmentsToReturn = true;
      this._PostTag = "</B>";
      this.__isset.PostTag = true;
      this._PreTag = "</B>";
      this.__isset.PreTag = true;
    }

    public HighlightOption(List<string> HighlightedFields) : this() {
      this.HighlightedFields = HighlightedFields;
    }

    public void Read (TProtocol iprot)
    {
      bool isset_HighlightedFields = false;
      TField field;
      iprot.ReadStructBegin();
      while (true)
      {
        field = iprot.ReadFieldBegin();
        if (field.Type == TType.Stop) { 
          break;
        }
        switch (field.ID)
        {
          case 1:
            if (field.Type == TType.I16) {
              FragmentsToReturn = iprot.ReadI16();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 2:
            if (field.Type == TType.List) {
              {
                HighlightedFields = new List<string>();
                TList _list14 = iprot.ReadListBegin();
                for( int _i15 = 0; _i15 < _list14.Count; ++_i15)
                {
                  string _elem16 = null;
                  _elem16 = iprot.ReadString();
                  HighlightedFields.Add(_elem16);
                }
                iprot.ReadListEnd();
              }
              isset_HighlightedFields = true;
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 3:
            if (field.Type == TType.String) {
              PostTag = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          case 4:
            if (field.Type == TType.String) {
              PreTag = iprot.ReadString();
            } else { 
              TProtocolUtil.Skip(iprot, field.Type);
            }
            break;
          default: 
            TProtocolUtil.Skip(iprot, field.Type);
            break;
        }
        iprot.ReadFieldEnd();
      }
      iprot.ReadStructEnd();
      if (!isset_HighlightedFields)
        throw new TProtocolException(TProtocolException.INVALID_DATA);
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("HighlightOption");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (__isset.FragmentsToReturn) {
        field.Name = "FragmentsToReturn";
        field.Type = TType.I16;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        oprot.WriteI16(FragmentsToReturn);
        oprot.WriteFieldEnd();
      }
      field.Name = "HighlightedFields";
      field.Type = TType.List;
      field.ID = 2;
      oprot.WriteFieldBegin(field);
      {
        oprot.WriteListBegin(new TList(TType.String, HighlightedFields.Count));
        foreach (string _iter17 in HighlightedFields)
        {
          oprot.WriteString(_iter17);
        }
        oprot.WriteListEnd();
      }
      oprot.WriteFieldEnd();
      if (PostTag != null && __isset.PostTag) {
        field.Name = "PostTag";
        field.Type = TType.String;
        field.ID = 3;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(PostTag);
        oprot.WriteFieldEnd();
      }
      if (PreTag != null && __isset.PreTag) {
        field.Name = "PreTag";
        field.Type = TType.String;
        field.ID = 4;
        oprot.WriteFieldBegin(field);
        oprot.WriteString(PreTag);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override bool Equals(object that) {
      var other = that as HighlightOption;
      if (other == null) return false;
      if (ReferenceEquals(this, other)) return true;
      return ((__isset.FragmentsToReturn == other.__isset.FragmentsToReturn) && ((!__isset.FragmentsToReturn) || (System.Object.Equals(FragmentsToReturn, other.FragmentsToReturn))))
        && TCollections.Equals(HighlightedFields, other.HighlightedFields)
        && ((__isset.PostTag == other.__isset.PostTag) && ((!__isset.PostTag) || (System.Object.Equals(PostTag, other.PostTag))))
        && ((__isset.PreTag == other.__isset.PreTag) && ((!__isset.PreTag) || (System.Object.Equals(PreTag, other.PreTag))));
    }

    public override int GetHashCode() {
      int hashcode = 0;
      unchecked {
        hashcode = (hashcode * 397) ^ (!__isset.FragmentsToReturn ? 0 : (FragmentsToReturn.GetHashCode()));
        hashcode = (hashcode * 397) ^ ((TCollections.GetHashCode(HighlightedFields)));
        hashcode = (hashcode * 397) ^ (!__isset.PostTag ? 0 : (PostTag.GetHashCode()));
        hashcode = (hashcode * 397) ^ (!__isset.PreTag ? 0 : (PreTag.GetHashCode()));
      }
      return hashcode;
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("HighlightOption(");
      sb.Append("FragmentsToReturn: ");
      sb.Append(FragmentsToReturn);
      sb.Append(",HighlightedFields: ");
      sb.Append(HighlightedFields);
      sb.Append(",PostTag: ");
      sb.Append(PostTag);
      sb.Append(",PreTag: ");
      sb.Append(PreTag);
      sb.Append(")");
      return sb.ToString();
    }

  }

}