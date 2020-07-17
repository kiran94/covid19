python -m grpc_tools.protoc -I. \
    --python_out=. \
    --grpc_python_out=. \
    --proto_path ../../api/Covid.Api.Grpc/Protos covid.proto

echo "Generated"