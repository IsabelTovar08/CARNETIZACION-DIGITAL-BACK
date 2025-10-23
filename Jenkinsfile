/// <summary>
/// Jenkinsfile CI/CD para CARNETIZACION-DIGITAL-BACK (.NET 8)
/// - Usa un solo contenedor Docker con SDK .NET 8.
/// - Ejecuta restore, build, publish y despliegue Docker.
/// - Incluye entorno configurado, rutas correctas y shell estable.
/// </summary>

pipeline {

    /// <summary>
    /// Configuración global del pipeline:
    /// Se usa una sola imagen .NET SDK 8 dentro de Docker.
    /// </summary>
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:8.0'
            // 🧩 Se montan el socket y el workspace completo
            args '-v /var/run/docker.sock:/var/run/docker.sock -v /var/jenkins_home/workspace:/var/jenkins_home/workspace -w /var/jenkins_home/workspace/carnetizacion-digital-api-prod'
            reuseNode true
        }
    }

    /// <summary>
    /// Jenkins usará /bin/sh (no bash) para evitar errores de proceso.
    /// </summary>
    options {
        shell '/bin/sh'
    }

    /// <summary>
    /// Variables globales de entorno.
    /// </summary>
    environment {
        DOTNET_CLI_HOME = '/tmp'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
        APP_NAME = 'carnetizacion-digital-api-prod'
        PORT = '5300'
    }

    stages {

        stage('Prepare Environment') {
            steps {
                echo '🧩 Preparando entorno dentro del contenedor...'
                // 🔧 Instala bash solo si se requiere
                sh '''
                    apt-get update -qq && apt-get install -y -qq bash > /dev/null
                    mkdir -p $DOTNET_CLI_HOME
                    chmod -R 777 $DOTNET_CLI_HOME
                '''
            }
        }

        stage('Restore') {
            steps {
                echo '🔧 Restaurando dependencias...'
                sh '''
                    echo "📁 Contenido del workspace:"
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
                echo '🐳 Construyendo imagen Docker...'
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
