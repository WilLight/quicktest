// список классов
// данные пользователя 

export const Classroom = () => {
    return (
        
        <div className="dashboard">
            <div className="dashboard__name"><h1>Classroom</h1></div>
            <div className="dashboard__header">

            <div className="dashboard__profile flex-column">
                <span>Quick Test Classroom</span>
                <span>Students: 2137</span>
            </div>
            <div className="dashboard__items">
                <button className="dashboard__button dashboard__button--active">Quizez</button>
                <button className="dashboard__button">Students</button>
                <button className="dashboard__button">Info</button>
            </div>
            </div>
            <ul className="dashboard__content">
                <li className="modules__card">
                    <h3>Quick Test</h3>
                    <span>2137 questions</span>
                </li>
                <li className="modules__card">
                    <h3>Dolor sunt ad reprehenderit labore velit mollit culpa sunt voluptate reprehenderit velit quis duis.</h3>
                    <span>307 questions</span>
                </li>
            </ul>
        </div>
         
    )
}
