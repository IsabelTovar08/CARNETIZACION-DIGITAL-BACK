pipeline {

    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:8.0'
            args '-v /var/run/docker.sock:/var/run/docker.sock -v /var/jenkins_home/workspace:/var/jenkins_home/workspace -w /var/jenkins_home/workspace/carnetizacion-digital-api-prod'
        }
    }

    environment {
        DOTNET_CLI_HOME = '/tmp'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
        APP_NAME = 'carnetizacion-digital-api-prod'
        PORT = '5300'
    }

    stages {

        stage('Restore') {
            steps {
                echo '🔧 Restaurando dependencias...'
                sh '''
                    mkdir -p /tmp
                    echo "📁 Contenido actual del workspace:"
                    ls -la
                    dotnet restore CARNETIZACION-DIGITAL-BACK.sln
                '''
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
