import React, { FunctionComponent } from 'react'

interface IProps {
  easy?: boolean
  normal?: boolean
  hard?: boolean
  expert?: boolean
  expertPlus?: boolean
}

export const DiffTags: FunctionComponent<IProps> = ({
  easy,
  normal,
  hard,
  expert,
  expertPlus,
}) => (
  <div className='tags'>
    {easy ? <span className='tag is-easy'>Easy</span> : null}
    {normal ? <span className='tag is-normal'>Normal</span> : null}
    {hard ? <span className='tag is-hard'>Hard</span> : null}
    {expert ? <span className='tag is-expert'>Expert</span> : null}
    {expertPlus ? <span className='tag is-expert-plus'>Expert+</span> : null}
  </div>
)
