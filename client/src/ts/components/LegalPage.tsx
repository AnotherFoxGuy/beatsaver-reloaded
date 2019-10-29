import { AxiosError } from 'axios'
import Markdown from 'markdown-to-jsx'
import React, { FunctionComponent, useEffect, useState } from 'react'
import { NotFound } from '../routes/NotFound'
import { axios } from '../utils/axios'
import { Loader } from './Loader'
import { TextPage } from './TextPage'

interface IProps {
  title: string
  url: string
}

type LoadState = 'loading' | 'loaded' | 'errored' | 'not-implemented'
const notImplemented = '@@loadState/NOT_IMPLEMENTED'

export const LegalPage: FunctionComponent<IProps> = ({ title, url }) => {
  const stored = sessionStorage.getItem(`legal:${url}`)

  const [content, setContent] = useState<string>(
    stored === null || stored === null ? '' : stored
  )

  const [state, setState] = useState<LoadState>(
    stored === null
      ? 'loading'
      : stored === notImplemented
      ? 'not-implemented'
      : 'loaded'
  )

  useEffect(() => {
    if (state !== 'loading') return

    axios
      .get(`/legal/${url}`)
      .then(resp => {
        setState('loaded')
        setContent(resp.data)
        sessionStorage.setItem(`legal:${url}`, resp.data)
      })
      .catch(e => {
        const err = e as AxiosError
        const resp = err.response

        if (resp && resp.status === 501) {
          sessionStorage.setItem(`legal:${url}`, notImplemented)
          setState('not-implemented')
        } else {
          setState('errored')
        }
      })
  }, [title, url])

  if (state === 'not-implemented') return <NotFound />
  if (state === 'loading') return <Loader />
  if (state === 'errored') {
    return (
      <TextPage title={title}>
        <p>Could not load page content, please contact a site administrator.</p>
      </TextPage>
    )
  }

  return (
    <TextPage title={title}>
      <div className='content'>
        <Markdown>{content}</Markdown>
      </div>
    </TextPage>
  )
}

export const DMCAPage = () => <LegalPage title='DMCA' url='dmca' />
export const PrivacyPage = () => <LegalPage title='Privacy' url='privacy' />
