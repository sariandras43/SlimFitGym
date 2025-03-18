export interface TrainingModel {
    id: number
    trainer: string
    trainerId: number
    room: string
    roomId: number
    name: string
    trainingStart: Date
    trainingEnd: Date
    maxPeople: number
    freePlaces: number
    isActive: boolean
    trainerImageUrl: string
    roomImageUrl: string
  }
  