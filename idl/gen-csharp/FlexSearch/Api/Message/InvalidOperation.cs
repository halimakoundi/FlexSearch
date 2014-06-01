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

namespace FlexSearch.Api.Message
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class InvalidOperation : TException, TBase
  {
    private OperationMessage _Message;

    [DataMember(Order = 1)]
    public OperationMessage Message
    {
      get
      {
        return _Message;
      }
      set
      {
        __isset.Message = true;
        this._Message = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    [DataContract]
    public struct Isset {
      public bool Message;
    }

    public InvalidOperation() {
    }

    public void Read (TProtocol iprot)
    {
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
            if (field.Type == TType.Struct) {
              Message = new OperationMessage();
              Message.Read(iprot);
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
    }

    public void Write(TProtocol oprot) {
      TStruct struc = new TStruct("InvalidOperation");
      oprot.WriteStructBegin(struc);
      TField field = new TField();
      if (Message != null && __isset.Message) {
        field.Name = "Message";
        field.Type = TType.Struct;
        field.ID = 1;
        oprot.WriteFieldBegin(field);
        Message.Write(oprot);
        oprot.WriteFieldEnd();
      }
      oprot.WriteFieldStop();
      oprot.WriteStructEnd();
    }

    public override bool Equals(object that) {
      var other = that as InvalidOperation;
      if (other == null) return false;
      if (ReferenceEquals(this, other)) return true;
      return ((__isset.Message == other.__isset.Message) && ((!__isset.Message) || (System.Object.Equals(Message, other.Message))));
    }

    public override int GetHashCode() {
      int hashcode = 0;
      unchecked {
        hashcode = (hashcode * 397) ^ (!__isset.Message ? 0 : (Message.GetHashCode()));
      }
      return hashcode;
    }

    public override string ToString() {
      StringBuilder sb = new StringBuilder("InvalidOperation(");
      sb.Append("Message: ");
      sb.Append(Message== null ? "<null>" : Message.ToString());
      sb.Append(")");
      return sb.ToString();
    }

  }


  #if !SILVERLIGHT
  [Serializable]
  #endif
  [DataContract]
  public partial class InvalidOperationFault
  {
    private OperationMessage _Message;

    [DataMember(Order = 2)]
    public OperationMessage Message
    {
      get
      {
        return _Message;
      }
      set
      {
        this._Message = value;
      }
    }

  }

}