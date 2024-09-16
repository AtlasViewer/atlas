pipeline {
    agent any

    options {
        timestamps()
        buildDiscarder(logRotator(numToKeepStr: '5'))
    }

    environment {
        IS_DEVELOPMENT_BUILD = true // Temporarily, all builds are dev builds. In the future only the dev branch will be development builds
        // A second flag will be added on first initial release: SNAPSHOT. Which will indicate if the viewer is a snapshot of the live branch or a build issued by the team.
    }

    stages {
        stage("Build Windows") {
            agent {
                label 'windows'
            }

            environment {
                BUILD_NAME = "Windows-${currentBuild.number}"
                String buildTarget = "Win64"
                String outputFolder = "out\\windows\\"
            }

            steps {
                script {
                    bat '''
                    @echo off

                    Unity -quit -projectPath "%cd%" -batchmode -buildTarget %buildTarget% -customBuildPath "%cd%\\%outputFolder%\\" -customBuildName "%BUILD_NAME%" -executeMethod BuildCommand.ProcessCLIBuild
                    '''
                }
            }
        }

        stage("Build Linux") {
            agent {
                label 'windows'
            }

            environment {
                BUILD_NAME = "Linux-${currentBuild.number}"
                String buildTarget = "Linux64"
                String outputFolder = "out\\linux\\"
            }

            steps {
                script {
                    bat '''
                    @echo off

                    Unity -quit -projectPath "%cd%" -batchmode -buildTarget %buildTarget% -customBuildPath "%cd%\\%outputFolder%\\" -customBuildName "%BUILD_NAME%" -executeMethod BuildCommand.ProcessCLIBuild
                    '''
                }
            }
        }
    }
}
