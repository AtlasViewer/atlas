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
        stage("Clean Environment") {
            agent {
                label 'windows'
            }

            steps {
                script {
                    bat '''

                    git clean -xfd
                    git reset --hard

                    mkdir out
                    mkdir out\\windows
                    mkdir out\\linux
                    mkdir out\\Android
                    mkdir out\\macOS
                    '''
                }
            }
        }
        stage("Build Windows") {
            agent {
                label 'windows'
            }

            environment {
                buildNumber = "${currentBuild.number}"
                String buildTarget = "Win64"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildNumber %buildNumber% -buildTarget %buildTarget% -outputPath "%cd%" -executeMethod BuildCommand.PerformWindows
                    '''

                    bat '''
                    cd out\\windows
                    tar -cvf ../../Windows.tgz .
                    '''
                }
            }

            post {
                always {
                    archiveArtifacts artifacts: "Windows.tgz"
                }
            }
        }

        stage("Build Linux") {
            agent {
                label 'windows'
            }

            environment {
                buildNumber = "${currentBuild.number}"
                String buildTarget = "Linux"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildNumber %buildNumber%  -buildTarget %buildTarget% -outputPath "%cd%" -executeMethod BuildCommand.PerformLinux
                    '''

                    bat '''
                    cd out\\linux
                    tar -cvf ../../Linux.tgz .
                    '''
                }
            }

            post {
                always {
                    archiveArtifacts artifacts: "Linux.tgz"
                }
            }
        }

        stage("Build MacOS") {
            agent {
                label 'windows'
            }

            environment {
                buildNumber = "${currentBuild.number}"
                String buildTarget = "macOS"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildNumber %buildNumber%  -buildTarget %buildTarget% -outputPath "%cd%" -executeMethod BuildCommand.PerformMacOS
                    '''

                    bat '''
                    cd out\\macOS
                    tar -cvf ../../MacOS.tgz .
                    '''
                }
            }

            post {
                always {
                    archiveArtifacts artifacts: "MacOS.tgz"
                }
            }
        }

        stage("Build Android") {
            agent {
                label 'windows'
            }

            environment {
                buildNumber = "${currentBuild.number}"
                String buildTarget = "Android"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildNumber %buildNumber% -buildTarget %buildTarget% -outputPath "%cd%" -executeMethod BuildCommand.PerformAndroid
                    '''
                }
            }

            post {
                always {
                    archiveArtifacts artifacts: "out\\android\\*.apk"
                }
            }
        }

        stage("Clean Up Build Folder") {
            agent {
                label 'windows'
            }

            steps {
                script {
                    bat '''
                    git clean -xfd
                    git reset --hard
                    '''
                }
            }

            post {
                always {
                    deleteDir()
                }
            }
        }
    }
}
