pipeline {

    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:8.0'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }

    environment {
        DOTNET_CLI_HOME = '/var/jenkins_home/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {

        stage('Prepare Permissions') {
            steps {
                /// <summary>
                /// Crea el directorio de caché .NET con permisos válidos.
                /// </summary>
                echo '🔧 Ajustando permisos para .NET CLI...'
                sh '''
                    mkdir -p $DOTNET_CLI_HOME
                    chmod -R 777 $DOTNET_CLI_HOME
                '''
            }
        }

        stage('Restore') {
            steps {
                echo '🔧 Restaurando dependencias...'
                sh 'dotnet restore CARNETIZACION-DIGITAL-BACK.sln'
            }
        }

        stage('Build') {
            steps {
                echo '🏗️ Compilando la solución...'
                sh '''
                    for proj in $(find . -name "*.csproj" ! -path "./Diagram/*"); do
                        dotnet build "$proj" --no-restore -c Release
                    done
                '''
            }
        }

        stage('Publish Web Layer') {
            steps {
                echo '📦 Publicando capa Web...'
                sh 'dotnet publish Web/Web.csproj -c Release -o ./publish'
            }
        }

        stage('Build Docker Image') {
            steps {
                echo '🐳 Construyendo imagen Docker...'
                sh '''
                    APP_NAME=carnetizacion-digital-back
                    docker build -t $APP_NAME:latest .
                '''
            }
        }

        stage('Deploy Docker Container') {
            steps {
                echo '🚀 Desplegando contenedor Docker...'
                sh '''
                    APP_NAME=carnetizacion-digital-back
                    docker stop $APP_NAME || true
                    docker rm $APP_NAME || true
                    docker run -d -p 5000:8080 --name $APP_NAME $APP_NAME:latest
                '''
            }
        }
    }

    post {
        success {
            echo '✅ Pipeline completado correctamente.'
        }
        failure {
            echo '❌ Error durante el proceso del pipeline.'
        }
    }
}
