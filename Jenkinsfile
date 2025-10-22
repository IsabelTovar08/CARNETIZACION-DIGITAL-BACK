/// <summary>
/// Jenkinsfile para automatizar el flujo CI/CD de la aplicación .NET 8 CARNETIZACION-DIGITAL-BACK.
/// Publica la capa Web, construye la imagen Docker y despliega automáticamente el contenedor.
/// </summary>

pipeline {

    /// <summary>
    /// Define que este pipeline puede ejecutarse en cualquier agente disponible.
    /// </summary>
     agent {
        docker {
            image 'mcr.microsoft.com/dotnet/sdk:8.0'
            args '-v /var/run/docker.sock:/var/run/docker.sock'
        }
    }

    stages {

        stage('Restore') {
            steps {
                /// <summary>
                /// Restaura las dependencias NuGet de toda la solución.
                /// </summary>
                echo '🔧 Restaurando dependencias...'
                sh 'dotnet restore CARNETIZACION-DIGITAL-BACK.sln'
            }
        }

        stage('Build') {
            steps {
                /// <summary>
                /// Compila toda la solución en modo Release sin restaurar nuevamente.
                /// </summary>
                echo '🏗️ Compilando la solución...'
                sh 'dotnet build CARNETIZACION-DIGITAL-BACK.sln --no-restore -c Release'
            }
        }

        stage('Test') {
            when {
                /// <summary>
                /// Solo ejecuta esta etapa si existe un proyecto de pruebas.
                /// </summary>
                expression { fileExists('Tests') }
            }
            steps {
                /// <summary>
                /// Ejecuta las pruebas unitarias si existen.
                /// </summary>
                echo '🧪 Ejecutando pruebas unitarias...'
                sh 'dotnet test --no-build --verbosity normal'
            }
        }

        stage('Publish Web Layer') {
            steps {
                /// <summary>
                /// Publica la capa Web del proyecto (API principal).
                /// </summary>
                echo '📦 Publicando capa Web...'
                sh 'dotnet publish Web/Web.csproj -c Release -o ./publish'
            }
        }

        stage('Build Docker Image') {
            when {
                /// <summary>
                /// Solo ejecuta si existe un Dockerfile.
                /// </summary>
                expression { fileExists('Dockerfile') }
            }
            steps {
                /// <summary>
                /// Construye una imagen Docker de la aplicación Web publicada.
                /// </summary>
                echo '🐳 Construyendo imagen Docker...'
                sh '''
                    APP_NAME=carnetizacion-digital-back
                    docker build -t $APP_NAME:latest .
                '''
            }
        }

        stage('Deploy Docker Container') {
            when {
                /// <summary>
                /// Solo despliega si existe la imagen Docker generada.
                /// </summary>
                expression { fileExists('Dockerfile') }
            }
            steps {
                /// <summary>
                /// Despliega el contenedor actualizando la versión en ejecución.
                /// </summary>
                echo '🚀 Desplegando contenedor Docker...'
                sh '''
                    APP_NAME=carnetizacion-digital-back
                    docker stop $APP_NAME || true
                    docker rm $APP_NAME || true
                    docker run -d -p 5000:8080 --name $APP_NAME $APP_NAME:latest
                '''
            }
        }

        stage('Archive Artifacts') {
            steps {
                /// <summary>
                /// Guarda los binarios publicados como artefactos dentro de Jenkins.
                /// </summary>
                echo '🗂️ Archivando artefactos...'
                archiveArtifacts artifacts: 'publish/**', fingerprint: true
            }
        }
    }

    post {
        success {
            /// <summary>
            /// Mensaje mostrado si el pipeline finaliza exitosamente.
            /// </summary>
            echo '✅ Pipeline completado y capa Web desplegada correctamente.'
        }
        failure {
            /// <summary>
            /// Mensaje mostrado si ocurre un error.
            /// </summary>
            echo '❌ Error durante el proceso del pipeline.'
        }
    }
}
