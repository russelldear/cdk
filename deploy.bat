# Don't forget to check you've done all the prerequisites in README.md

pushd src

dotnet lambda package -o ../publish/SinglePageApp.zip

popd

pushd infra

cdk deploy

popd