/// <summary>
/// Jenkinsfile principal para despliegue automatizado del proyecto carnetizacion-digital-api.
/// Detecta el entorno desde el archivo .env raíz, compila el proyecto .NET 8,
/// y ejecuta el docker-compose correspondiente dentro de devops/{entorno}.
/// Evita el bug del plugin DockerWorkflow ejecutando contenedores SDK manualmente.
/// </summary>

pipeline {
    agent any

    options {
        skipDefaultCheckout(true)
        timestamps()
    }

    environment {
        DOTNET_CLI_HOME = '/var/jenkins_home/.dotnet'
        DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
        DOTNET_NOLOGO = '1'
    }

    stages {

        /// <summary>
        /// Etapa 1: Detectar entorno desde .env raíz.
        /// </summary>
        stage('Leer entorno desde .env raíz') {
            steps {
                script {
                    def envValue = sh(script: "grep '^ENVIRONMENT=' .env | cut -d '=' -f2", returnStdout: true).trim()
                    if (envValue == '') {
                        error "❌ No se encontró ENVIRONMENT en el archivo .env raíz"
                    }

                    env.ENVIRONMENT = envValue
                    env.ENV_DIR = "devops/${env.ENVIRONMENT}"
                    env.COMPOSE_FILE = "${env.ENV_DIR}/docker-compose.yml"
                    env.ENV_FILE = "${env.ENV_DIR}/.env"

                    echo "🌍 Entorno detectado: ${env.ENVIRONMENT}"
                    echo "📄 Archivo compose: ${env.COMPOSE_FILE}"
                    echo "⚙️ Archivo de entorno: ${env.ENV_FILE}"
                }
            }
        }

        /// <summary>
        /// Etapa 2: Restaurar dependencias con SDK de .NET dentro de contenedor.
        /// </summary>
        stage('Restaurar dependencias') {
            steps {
                sh '''
                    echo "📦 Restaurando dependencias dentro de contenedor .NET SDK..."
                    docker run --rm \
                        -v $PWD:/src \
                        -w /src \
                        -e DOTNET_CLI_HOME=$DOTNET_CLI_HOME \
                        -e DOTNET_SKIP_FIRST_TIME_EXPERIENCE=$DOTNET_SKIP_FIRST_TIME_EXPERIENCE \
                        -e DOTNET_NOLOGO=$DOTNET_NOLOGO \
                        mcr.microsoft.com/dotnet/sdk:8.0 \
                        bash -c "mkdir -p $DOTNET_CLI_HOME && chmod -R 777 $DOTNET_CLI_HOME && dotnet restore Web/Web.csproj"
                '''
            }
        }

        /// <summary>
        /// Etapa 3: Compilar el proyecto con .NET SDK 8.0.
        /// </summary>
        stage('Compilar proyecto') {
            steps {
                sh '''
                    echo "🛠️ Compilando proyecto dentro del contenedor .NET SDK..."
                    docker run --rm \
                        -v $PWD:/src \
                        -w /src \
                        -e DOTNET_CLI_HOME=$DOTNET_CLI_HOME \
                        -e DOTNET_SKIP_FIRST_TIME_EXPERIENCE=$DOTNET_SKIP_FIRST_TIME_EXPERIENCE \
                        -e DOTNET_NOLOGO=$DOTNET_NOLOGO \
                        mcr.microsoft.com/dotnet/sdk:8.0 \
                        bash -c "dotnet build Web/Web.csproj --configuration Release"
                '''
            }
        }

        /// <summary>
        /// Etapa 4: Desplegar API con docker-compose del entorno detectado.
        /// Limpia contenedores e imágenes antiguas para evitar conflictos.
        /// </summary>
        stage('Desplegar API') {
            steps {
                sh """
                    echo "🧹 Limpiando contenedores antiguos..."
                    docker ps -a --filter "name=carnetizacion-digital-api-${env.ENVIRONMENT}" -q | xargs -r docker rm -f || true
                    docker images "carnetizacion-digital-api-${env.ENVIRONMENT}" -q | xargs -r docker rmi -f || true
                    docker system prune -f --volumes || true

                    echo "🚀 Levantando servicios..."
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
