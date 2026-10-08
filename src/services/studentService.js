import { students } from '../data/mockData'
export const getStudents = async () => Promise.resolve(students)
export const getStudent = async (id) => Promise.resolve(students.find((student) => student.id === id))
