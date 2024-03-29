# -*- coding: utf-8 -*-
# Generated by the protocol buffer compiler.  DO NOT EDIT!
# source: covid.proto

from google.protobuf import descriptor as _descriptor
from google.protobuf import message as _message
from google.protobuf import reflection as _reflection
from google.protobuf import symbol_database as _symbol_database
# @@protoc_insertion_point(imports)

_sym_db = _symbol_database.Default()


from google.protobuf import timestamp_pb2 as google_dot_protobuf_dot_timestamp__pb2


DESCRIPTOR = _descriptor.FileDescriptor(
  name='covid.proto',
  package='covid',
  syntax='proto3',
  serialized_options=b'\252\002\016Covid.Api.Grpc',
  create_key=_descriptor._internal_create_key,
  serialized_pb=b'\n\x0b\x63ovid.proto\x12\x05\x63ovid\x1a\x1fgoogle/protobuf/timestamp.proto\"\xcf\x01\n\x0c\x43ovidRequest\x12\x15\n\rCountryRegion\x18\x01 \x01(\t\x12\x15\n\rProvinceState\x18\x02 \x01(\t\x12\x0e\n\x06\x43ounty\x18\x03 \x01(\t\x12\x0e\n\x06\x46ields\x18\x04 \x03(\t\x12\x33\n\rabsoluteDates\x18\x05 \x01(\x0b\x32\x1a.covid.AbsoluteDateRequestH\x00\x12\x33\n\rrelativeDates\x18\x06 \x01(\x0b\x32\x1a.covid.RelativeDateRequestH\x00\x42\x07\n\x05\x44\x61tes\"1\n\x13RelativeDateRequest\x12\r\n\x05Start\x18\x01 \x01(\x05\x12\x0b\n\x03\x45nd\x18\x02 \x01(\x05\"@\n\x13\x41\x62soluteDateRequest\x12)\n\x05\x44\x61tes\x18\x01 \x03(\x0b\x32\x1a.google.protobuf.Timestamp\"\x95\x01\n\rCovidResponse\x12\x15\n\rCountryRegion\x18\x01 \x01(\t\x12\x15\n\rProvinceState\x18\x02 \x01(\t\x12\x0e\n\x06\x43ounty\x18\x03 \x01(\t\x12\r\n\x05\x46ield\x18\x04 \x01(\t\x12(\n\x04\x44\x61te\x18\x05 \x01(\x0b\x32\x1a.google.protobuf.Timestamp\x12\r\n\x05Value\x18\x06 \x01(\x01\"A\n\x17\x43ovidResponseCollection\x12&\n\x08Response\x18\x01 \x03(\x0b\x32\x14.covid.CovidResponse2J\n\x0c\x43ovidService\x12:\n\x03Get\x12\x13.covid.CovidRequest\x1a\x1e.covid.CovidResponseCollectionB\x11\xaa\x02\x0e\x43ovid.Api.Grpcb\x06proto3'
  ,
  dependencies=[google_dot_protobuf_dot_timestamp__pb2.DESCRIPTOR,])




_COVIDREQUEST = _descriptor.Descriptor(
  name='CovidRequest',
  full_name='covid.CovidRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='CountryRegion', full_name='covid.CovidRequest.CountryRegion', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='ProvinceState', full_name='covid.CovidRequest.ProvinceState', index=1,
      number=2, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='County', full_name='covid.CovidRequest.County', index=2,
      number=3, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='Fields', full_name='covid.CovidRequest.Fields', index=3,
      number=4, type=9, cpp_type=9, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='absoluteDates', full_name='covid.CovidRequest.absoluteDates', index=4,
      number=5, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='relativeDates', full_name='covid.CovidRequest.relativeDates', index=5,
      number=6, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
    _descriptor.OneofDescriptor(
      name='Dates', full_name='covid.CovidRequest.Dates',
      index=0, containing_type=None,
      create_key=_descriptor._internal_create_key,
    fields=[]),
  ],
  serialized_start=56,
  serialized_end=263,
)


_RELATIVEDATEREQUEST = _descriptor.Descriptor(
  name='RelativeDateRequest',
  full_name='covid.RelativeDateRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='Start', full_name='covid.RelativeDateRequest.Start', index=0,
      number=1, type=5, cpp_type=1, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='End', full_name='covid.RelativeDateRequest.End', index=1,
      number=2, type=5, cpp_type=1, label=1,
      has_default_value=False, default_value=0,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=265,
  serialized_end=314,
)


_ABSOLUTEDATEREQUEST = _descriptor.Descriptor(
  name='AbsoluteDateRequest',
  full_name='covid.AbsoluteDateRequest',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='Dates', full_name='covid.AbsoluteDateRequest.Dates', index=0,
      number=1, type=11, cpp_type=10, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=316,
  serialized_end=380,
)


_COVIDRESPONSE = _descriptor.Descriptor(
  name='CovidResponse',
  full_name='covid.CovidResponse',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='CountryRegion', full_name='covid.CovidResponse.CountryRegion', index=0,
      number=1, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='ProvinceState', full_name='covid.CovidResponse.ProvinceState', index=1,
      number=2, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='County', full_name='covid.CovidResponse.County', index=2,
      number=3, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='Field', full_name='covid.CovidResponse.Field', index=3,
      number=4, type=9, cpp_type=9, label=1,
      has_default_value=False, default_value=b"".decode('utf-8'),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='Date', full_name='covid.CovidResponse.Date', index=4,
      number=5, type=11, cpp_type=10, label=1,
      has_default_value=False, default_value=None,
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
    _descriptor.FieldDescriptor(
      name='Value', full_name='covid.CovidResponse.Value', index=5,
      number=6, type=1, cpp_type=5, label=1,
      has_default_value=False, default_value=float(0),
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=383,
  serialized_end=532,
)


_COVIDRESPONSECOLLECTION = _descriptor.Descriptor(
  name='CovidResponseCollection',
  full_name='covid.CovidResponseCollection',
  filename=None,
  file=DESCRIPTOR,
  containing_type=None,
  create_key=_descriptor._internal_create_key,
  fields=[
    _descriptor.FieldDescriptor(
      name='Response', full_name='covid.CovidResponseCollection.Response', index=0,
      number=1, type=11, cpp_type=10, label=3,
      has_default_value=False, default_value=[],
      message_type=None, enum_type=None, containing_type=None,
      is_extension=False, extension_scope=None,
      serialized_options=None, file=DESCRIPTOR,  create_key=_descriptor._internal_create_key),
  ],
  extensions=[
  ],
  nested_types=[],
  enum_types=[
  ],
  serialized_options=None,
  is_extendable=False,
  syntax='proto3',
  extension_ranges=[],
  oneofs=[
  ],
  serialized_start=534,
  serialized_end=599,
)

_COVIDREQUEST.fields_by_name['absoluteDates'].message_type = _ABSOLUTEDATEREQUEST
_COVIDREQUEST.fields_by_name['relativeDates'].message_type = _RELATIVEDATEREQUEST
_COVIDREQUEST.oneofs_by_name['Dates'].fields.append(
  _COVIDREQUEST.fields_by_name['absoluteDates'])
_COVIDREQUEST.fields_by_name['absoluteDates'].containing_oneof = _COVIDREQUEST.oneofs_by_name['Dates']
_COVIDREQUEST.oneofs_by_name['Dates'].fields.append(
  _COVIDREQUEST.fields_by_name['relativeDates'])
_COVIDREQUEST.fields_by_name['relativeDates'].containing_oneof = _COVIDREQUEST.oneofs_by_name['Dates']
_ABSOLUTEDATEREQUEST.fields_by_name['Dates'].message_type = google_dot_protobuf_dot_timestamp__pb2._TIMESTAMP
_COVIDRESPONSE.fields_by_name['Date'].message_type = google_dot_protobuf_dot_timestamp__pb2._TIMESTAMP
_COVIDRESPONSECOLLECTION.fields_by_name['Response'].message_type = _COVIDRESPONSE
DESCRIPTOR.message_types_by_name['CovidRequest'] = _COVIDREQUEST
DESCRIPTOR.message_types_by_name['RelativeDateRequest'] = _RELATIVEDATEREQUEST
DESCRIPTOR.message_types_by_name['AbsoluteDateRequest'] = _ABSOLUTEDATEREQUEST
DESCRIPTOR.message_types_by_name['CovidResponse'] = _COVIDRESPONSE
DESCRIPTOR.message_types_by_name['CovidResponseCollection'] = _COVIDRESPONSECOLLECTION
_sym_db.RegisterFileDescriptor(DESCRIPTOR)

CovidRequest = _reflection.GeneratedProtocolMessageType('CovidRequest', (_message.Message,), {
  'DESCRIPTOR' : _COVIDREQUEST,
  '__module__' : 'covid_pb2'
  # @@protoc_insertion_point(class_scope:covid.CovidRequest)
  })
_sym_db.RegisterMessage(CovidRequest)

RelativeDateRequest = _reflection.GeneratedProtocolMessageType('RelativeDateRequest', (_message.Message,), {
  'DESCRIPTOR' : _RELATIVEDATEREQUEST,
  '__module__' : 'covid_pb2'
  # @@protoc_insertion_point(class_scope:covid.RelativeDateRequest)
  })
_sym_db.RegisterMessage(RelativeDateRequest)

AbsoluteDateRequest = _reflection.GeneratedProtocolMessageType('AbsoluteDateRequest', (_message.Message,), {
  'DESCRIPTOR' : _ABSOLUTEDATEREQUEST,
  '__module__' : 'covid_pb2'
  # @@protoc_insertion_point(class_scope:covid.AbsoluteDateRequest)
  })
_sym_db.RegisterMessage(AbsoluteDateRequest)

CovidResponse = _reflection.GeneratedProtocolMessageType('CovidResponse', (_message.Message,), {
  'DESCRIPTOR' : _COVIDRESPONSE,
  '__module__' : 'covid_pb2'
  # @@protoc_insertion_point(class_scope:covid.CovidResponse)
  })
_sym_db.RegisterMessage(CovidResponse)

CovidResponseCollection = _reflection.GeneratedProtocolMessageType('CovidResponseCollection', (_message.Message,), {
  'DESCRIPTOR' : _COVIDRESPONSECOLLECTION,
  '__module__' : 'covid_pb2'
  # @@protoc_insertion_point(class_scope:covid.CovidResponseCollection)
  })
_sym_db.RegisterMessage(CovidResponseCollection)


DESCRIPTOR._options = None

_COVIDSERVICE = _descriptor.ServiceDescriptor(
  name='CovidService',
  full_name='covid.CovidService',
  file=DESCRIPTOR,
  index=0,
  serialized_options=None,
  create_key=_descriptor._internal_create_key,
  serialized_start=601,
  serialized_end=675,
  methods=[
  _descriptor.MethodDescriptor(
    name='Get',
    full_name='covid.CovidService.Get',
    index=0,
    containing_service=None,
    input_type=_COVIDREQUEST,
    output_type=_COVIDRESPONSECOLLECTION,
    serialized_options=None,
    create_key=_descriptor._internal_create_key,
  ),
])
_sym_db.RegisterServiceDescriptor(_COVIDSERVICE)

DESCRIPTOR.services_by_name['CovidService'] = _COVIDSERVICE

# @@protoc_insertion_point(module_scope)
