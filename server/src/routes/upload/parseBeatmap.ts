import AdmZIP from 'adm-zip'
import { createHash } from 'crypto'
import { validJSON } from '../../utils/json'
import { getDataPromise } from '../../utils/zip'

export const parseBeatmap: (
  zipBuf: Buffer
) => Promise<IParsedBeatmap> = async zipBuf => {
  const zip = new AdmZIP(zipBuf)

  const info = zip.getEntry('info.dat')
  if (info === null) {
    throw new Error('info.dat not found')
  }

  const infoDAT = await getDataPromise(info, true)
  if (!validJSON(infoDAT)) throw new Error('Invalid info.dat')
  const infoJSON: IBeatmapInfo = JSON.parse(infoDAT)

  const difficulties = ([] as IDifficultyBeatmap[]).concat(
    ...infoJSON._difficultyBeatmapSets.map(x => x._difficultyBeatmaps)
  )

  for (const diff of difficulties) {
    const diffEntry = zip.getEntry(diff._beatmapFilename)
    if (diffEntry === null) {
      throw new Error(`${diff._beatmapFilename} not found`)
    }
  }

  const diffBuffers = await Promise.all(
    difficulties.map(x => getDataPromise(zip.getEntry(x._beatmapFilename)))
  )

  const hash = createHash('sha1')
  hash.update(infoDAT)
  for (const b of diffBuffers) {
    hash.update(b)
  }

  const sha1 = hash.digest('hex')
  const parsed: IParsedBeatmap = {
    hash: sha1,

    metadata: {
      levelAuthorName: infoJSON._levelAuthorName,
      songAuthorName: infoJSON._songAuthorName,
      songName: infoJSON._songName,
      songSubName: infoJSON._songSubName,

      bpm: infoJSON._beatsPerMinute,
    },
  }

  return parsed
}
