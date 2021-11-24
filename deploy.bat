# Don't forget to chek your prerequisites in README.md

cd src

dotnet lambda package -o ../publish/SinglePageApp.zip

cd ../infra

cdk deploy