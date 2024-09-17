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
                    mkdir out\\android
                    mkdir out\\macOS
                    '''
                }
            }

            post {
                always {


                    withCredentials([string(credentialsId: "AtlasViewerDiscordWebHook", variable: "webhook")]) {

                        discordSend customAvatarUrl: '', customFile: '', customUsername: '', description: 'Build Starting', enableArtifactsList: false, footer: '', image: '', link: env.BUILD_URL, result: currentBuild.currentResult, scmWebUrl: 'https://github.com/AtlasViewer/atlas', thumbnail: '', title: 'Atlas Viewer Automatic Builds', webhookURL: WEBHOK_URL
                    }
                }
            }
        }
        stage("Build Windows") {
            agent {
                label 'windows'
            }

            environment {
                buildNumber = "${currentBuild.number}"
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildNumber %buildNumber% -outputPath "%cd%" -executeMethod BuildCommand.PerformWindows
                    '''

                    bat '''
                    cd out\\windows
                    del /S /Q *DoNotShip* || true
                    rmdir /S /Q *DoNotShip* || true
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
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildNumber %buildNumber% -outputPath "%cd%" -executeMethod BuildCommand.PerformLinux
                    '''

                    bat '''
                    cd out\\linux
                    del /S /Q *DoNotShip* || true
                    rmdir /S /Q *DoNotShip* || true
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
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildNumber %buildNumber% -outputPath "%cd%" -executeMethod BuildCommand.PerformMacOS
                    '''

                    bat '''
                    cd out\\macOS
                    del /S /Q *DoNotShip* || true
                    rmdir /S /Q *DoNotShip* || true
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
            }

            steps {
                script {
                    bat '''

                    Unity -quit -projectPath "%cd%" -logfile - -batchmode -buildNumber %buildNumber% -outputPath "%cd%" -executeMethod BuildCommand.PerformAndroid
                    '''

                    bat '''
                    cd out\\android
                    del /S /Q *DoNotShip* || true
                    rmdir /S /Q *DoNotShip* || true
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

                    withCredentials([string(credentialsId: "AtlasViewerDiscordWebHook", variable: "webhook")]) {

                        discordSend customAvatarUrl: '', customFile: '', customUsername: '', description: 'Build Completed', enableArtifactsList: true, footer: '', image: '', link: env.BUILD_URL, result: currentBuild.currentResult, scmWebUrl: 'https://github.com/AtlasViewer/atlas', thumbnail: '', title: 'Atlas Viewer Automatic Builds', webhookURL: WEBHOOK_URL
                    }

                    deleteDir()
                }
            }
        }
    }
}
