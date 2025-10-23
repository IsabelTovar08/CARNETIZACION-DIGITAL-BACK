pipeline {
    agent any

    environment {
        // Carpeta temporal segura para .NET
        DOTNET_CLI_HOME = '/tmp'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'

        APP_NAME = 'carnetizacion-digital-api-prod'
        PORT = '5300'
    }

    stages {

        stage('Restore') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args '-v /var/run/docker.sock:/var/run/docker.sock -v $WORKSPACE:$WORKSPACE -w $WORKSPACE'
                }
            }
            steps {
                echo '🔧 Restaurando dependencias...'
                sh '''
                    export DOTNET_CLI_HOME=/tmp
                    mkdir -p /tmp
                    dotnet restore CARNETIZACION-DIGITAL-BACK.sln
                '''
            }
        }

        stage('Build') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args '-v $WORKSPACE:$WORKSPACE -w $WORKSPACE'
                }
            }
            steps {
                echo '🏗️ Compilando la solución...'
                sh '''
                    export DOTNET_CLI_HOME=/tmp
                    for proj in $(find . -name "*.csproj" ! -path "./Diagram/*"); do
                        dotnet build "$proj" --no-restore -c Release
                    done
                '''
            }
        }

        stage('Publish Web Layer') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args '-v $WORKSPACE:$WORKSPACE -w $WORKSPACE'
                }
            }
            steps {
                echo '📦 Publicando capa Web...'
                sh '''
                    export DOTNET_CLI_HOME=/tmp
                    dotnet publish Web/Web.csproj -c Release -o ./publish
                '''
            }
        }

        stage('Build Docker Image') {
            steps {
                echo '🐳 Construyendo imagen Docker PRODUCCIÓN...'
                sh '''
                    docker build -t $APP_NAME:latest .
                '''
            }
        }

        stage('Deploy Docker Container') {
            steps {
                echo '🚀 Desplegando contenedor PRODUCCIÓN...'
                sh '''
                    docker stop $APP_NAME || true
                    docker rm $APP_NAME || true
                    docker run -d -p $PORT:8080 --name $APP_NAME $APP_NAME:latest
                '''
            }
        }
    }

    post {
        success {
            echo '✅ Pipeline PRODUCCIÓN completado correctamente.'
        }
        failure {
            echo '❌ Error en pipeline PRODUCCIÓN.'
        }
    }
}
