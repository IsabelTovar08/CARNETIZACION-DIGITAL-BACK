/// <summary>
/// Jenkinsfile principal para despliegue automatizado del proyecto carnetizacion-digital-api.
/// Este pipeline detecta el entorno desde el archivo .env raíz,
/// compila el proyecto .NET 8 y ejecuta el docker-compose correspondiente dentro de la carpeta devops/{entorno}.
/// Antes del despliegue, elimina cualquier contenedor previo con el mismo nombre para evitar conflictos.
/// </summary>

pipeline {
    agent any

    environment {
        DOTNET_CLI_HOME = '/var/jenkins_home/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {

        /// <summary>
        /// Etapa 1: Detección del entorno.
        /// </summary>
        stage('Leer entorno desde .env raíz') {
            steps {
                script {
                    def envValue = sh(script: "grep '^ENVIRONMENT=' .env | cut -d '=' -f2", returnStdout: true).trim()
                    if (envValue == '') {
                        error "No se encontró ENVIRONMENT en el archivo .env raíz"
                    }

                    env.ENVIRONMENT = envValue
                    env.ENV_DIR = "devops/${env.ENVIRONMENT}"
                    env.COMPOSE_FILE = "${env.ENV_DIR}/docker-compose.yml"
                    env.ENV_FILE = "${env.ENV_DIR}/.env"

                    echo "Entorno detectado: ${env.ENVIRONMENT}"
                    echo "Archivo compose: ${env.COMPOSE_FILE}"
                    echo "Archivo de entorno: ${env.ENV_FILE}"
                }
            }
        }

        /// <summary>
        /// Etapa 2: Restauración de dependencias (.NET SDK 8.0)
        /// </summary>
        stage('Restaurar dependencias') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args '-v /var/run/docker.sock:/var/run/docker.sock --entrypoint=tail mcr.microsoft.com/dotnet/sdk:8.0 -f /dev/null'
                }
            }
            steps {
                sh '''
                    echo "📦 Restaurando dependencias..."
                    mkdir -p $DOTNET_CLI_HOME
                    chmod -R 777 $DOTNET_CLI_HOME
                    dotnet restore Web/Web.csproj
                '''
            }
        }

        /// <summary>
        /// Etapa 3: Compilación del proyecto (.NET SDK 8.0)
        /// </summary>
        stage('Compilar proyecto') {
            agent {
                docker {
                    image 'mcr.microsoft.com/dotnet/sdk:8.0'
                    args '--entrypoint=tail mcr.microsoft.com/dotnet/sdk:8.0 -f /dev/null'
                }
            }
            steps {
                echo '🛠️ Compilando la solución carnetizacion-digital-api...'
                sh 'dotnet build Web/Web.csproj --configuration Release'
            }
        }

        /// <summary>
        /// Etapa 4: Despliegue del backend (Docker Compose)
        /// </summary>
        stage('Desplegar API') {
            steps {
                echo "🚀 Desplegando carnetizacion-digital-api para entorno: ${env.ENVIRONMENT}"

                sh """
                    echo "🧹 Limpiando contenedores e imágenes antiguas..."
                    docker ps -a --filter "name=carnetizacion-digital-api-${env.ENVIRONMENT}" -q | xargs -r docker rm -f || true
                    docker images "carnetizacion-digital-api-${env.ENVIRONMENT}" -q | xargs -r docker rmi -f || true
                    docker system prune -f --volumes || true

                    echo "🚀 Ejecutando nuevo despliegue limpio..."
                    docker compose -f ${env.COMPOSE_FILE} --env-file ${env.ENV_FILE} up -d --build --force-recreate --no-deps --remove-orphans
                """
            }
        }
    }

    post {
        success {
            echo "✅ Despliegue completado correctamente para ${env.ENVIRONMENT}"
        }
        failure {
            echo "❌ Error durante el despliegue en ${env.ENVIRONMENT}"
        }
    }
}
