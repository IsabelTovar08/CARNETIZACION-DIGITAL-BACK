/// <summary>
/// Jenkinsfile estable para el despliegue automatizado del proyecto carnetizacion-digital-api.
/// Usa la misma imagen base de build que tu Dockerfile (con SDK 8.0 preinstalado).
/// Evita el error "No .NET SDKs were found" ejecutando restauración y compilación en la misma base.
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
        BUILD_IMAGE = 'ubuntu-dotnet-sdk-8.0' // 👈 nombre personalizado de la imagen base
    }

    stages {

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

        stage('Restaurar dependencias') {
            steps {
                sh '''
                    echo "📦 Restaurando dependencias dentro de la imagen de build personalizada..."
                    docker build --target build -t $BUILD_IMAGE -f Dockerfile .
                    docker run --rm \
                        -v "$PWD:/src" \
                        -w /src \
                        -e DOTNET_CLI_HOME=$DOTNET_CLI_HOME \
                        -e DOTNET_SKIP_FIRST_TIME_EXPERIENCE=$DOTNET_SKIP_FIRST_TIME_EXPERIENCE \
                        -e DOTNET_NOLOGO=$DOTNET_NOLOGO \
                        $BUILD_IMAGE \
                        bash -c "dotnet restore Web/Web.csproj"
                '''
            }
        }

        stage('Compilar proyecto') {
            steps {
                sh '''
                    echo "🛠️ Compilando proyecto dentro de la misma imagen de build..."
                    docker run --rm \
                        -v "$PWD:/src" \
                        -w /src \
                        -e DOTNET_CLI_HOME=$DOTNET_CLI_HOME \
                        -e DOTNET_SKIP_FIRST_TIME_EXPERIENCE=$DOTNET_SKIP_FIRST_TIME_EXPERIENCE \
                        -e DOTNET_NOLOGO=$DOTNET_NOLOGO \
                        $BUILD_IMAGE \
                        bash -c "dotnet build Web/Web.csproj --configuration Release"
                '''
            }
        }

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
