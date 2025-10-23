pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = '/var/jenkins_home/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
        APP_NAME = 'carnetizacion-digital-api-prod'
        PORT = '5300'
    }

    stages {
        stage('Restore') {
            agent {
                docker { image 'mcr.microsoft.com/dotnet/sdk:8.0' }
            }
            steps {
                echo '🔧 Restaurando dependencias PRODUCCIÓN...'
                sh 'dotnet restore CARNETIZACION-DIGITAL-BACK.sln'
            }
        }

        stage('Build') {
            agent {
                docker { image 'mcr.microsoft.com/dotnet/sdk:8.0' }
            }
            steps {
                echo '🏗️ Compilando la solución PRODUCCIÓN...'
                sh 'dotnet build CARNETIZACION-DIGITAL-BACK.sln -c Release --no-restore'
            }
        }

        stage('Publish Web Layer') {
            steps {
                echo '📦 Publicando capa Web PRODUCCIÓN...'
                sh 'dotnet publish Web/Web.csproj -c Release -o ./publish'
            }
        }

        stage('Build Docker Image') {
            steps {
                echo '🐳 Construyendo imagen Docker PRODUCCIÓN...'
                sh 'docker build -t $APP_NAME:latest .'
            }
        }

        stage('Deploy Docker Container') {
            steps {
                echo '🚀 Desplegando contenedor PRODUCCIÓN...'
                sh '''
                    docker stop $APP_NAME || true
                    docker rm $APP_NAME || true
                    docker run -d -p $PORT:8080 --restart always --name $APP_NAME $APP_NAME:latest
                '''
            }
        }
    }

    post {
        success { echo '✅ Despliegue PRODUCCIÓN exitoso.' }
        failure { echo '❌ Error durante el despliegue PRODUCCIÓN.' }
    }
}
