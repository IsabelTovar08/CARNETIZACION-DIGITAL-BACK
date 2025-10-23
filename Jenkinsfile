/// <summary>
/// Jenkinsfile para automatizar el flujo CI/CD de la aplicación .NET 8 CARNETIZACION-DIGITAL-BACK.
/// Publica la capa Web, construye la imagen Docker y despliega automáticamente el contenedor.
/// </summary>

pipeline {

    /// <summary>
    /// Define el agente que usará la imagen oficial de .NET 8 SDK dentro de Docker.
    /// </summary>
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:8.0'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }

    /// <summary>
    /// Variables de entorno globales para todo el pipeline.
    /// </summary>
    environment {
        DOTNET_CLI_HOME = '/tmp'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {

        stage('Restore') {
            steps {
                /// <summary>
                /// Restaura las dependencias NuGet.
                /// </summary>
                echo '🔧 Restaurando dependencias...'
                sh 'dotnet restore CARNETIZACION-DIGITAL-BACK.sln'
            }
        }

        stage('Build') {
            steps {
                /// <summary>
                /// Compila la solución.
                /// </summary>
                echo '🏗️ Compilando la solución...'
                sh  '''
                        for proj in $(find . -name "*.csproj" ! -path "./Diagram/*"); do
                        dotnet build "$proj" --no-restore -c Release
                        done
                    '''

            }
        }

        stage('Publish Web Layer') {
            steps {
                /// <summary>
                /// Publica la capa Web en modo Release.
                /// </summary>
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
            echo 'Pipeline completado correctamente.'
        }
        failure {
            echo 'Error durante el proceso del pipeline.'
        }
    }
}
