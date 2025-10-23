pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = '/var/jenkins_home/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
        APP_NAME = 'carnetizacion-digital-api-staging'
        PORT = '5200'
    }

    stages {
        stage('Restore') {
            agent {
                docker { image 'mcr.microsoft.com/dotnet/sdk:8.0' }
            }
            steps {
                echo '🔧 Restaurando dependencias STAGING...'
                sh 'dotnet restore CARNETIZACION-DIGITAL-BACK.sln'
            }
        }

        stage('Build') {
            agent {
                docker { image 'mcr.microsoft.com/dotnet/sdk:8.0' }
            }
            steps {
                echo '🏗️ Compilando la solución STAGING...'
                sh 'dotnet build CARNETIZACION-DIGITAL-BACK.sln -c Release --no-restore'
            }
        }

        stage('Publish Web Layer') {
            steps {
                echo '📦 Publicando capa Web STAGING...'
                 sh '''
                    for proj in $(find . -name "*.csproj" ! -path "./Diagram/*"); do
                        dotnet build "$proj" --no-restore -c Release
                    done
                '''
            }
        }

        stage('Build Docker Image') {
            steps {
                echo '🐳 Construyendo imagen Docker STAGING...'
                sh 'docker build -t $APP_NAME:latest .'
            }
        }

        stage('Deploy Docker Container') {
            steps {
                echo '🚀 Desplegando contenedor STAGING...'
                sh '''
                    docker stop $APP_NAME || true
                    docker rm $APP_NAME || true
                    docker run -d -p $PORT:8080 --name $APP_NAME $APP_NAME:latest
                '''
            }
        }
    }

    post {
        success { echo '✅ Pipeline STAGING completado correctamente.' }
        failure { echo '❌ Error en pipeline STAGING.' }
    }
}
