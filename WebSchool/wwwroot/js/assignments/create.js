const questionsSection = document.getElementById('questionsSection');
const questions = [];

const addNewQuestion = () => {
    const question = createQuestionHeaders();
    addAnswerField(question);
}

const createQuestionHeaders = () => {
    const questionContainer = document.createElement('div');
    questionContainer.classList.add('mb-3');

    // Question number label
    const questionNumber = document.createElement('label');
    questionNumber.classList.add('form-label', 'd-block');
    questionNumber.textContent = `#${questions.length  + 1} Question`;
    questionContainer.appendChild(questionNumber);

    // question title container
    const questionTitleContainer = document.createElement('div');
    questionTitleContainer.classList.add('mb-3');
    questionContainer.appendChild(questionTitleContainer);

    // multiple answers label
    const multipleAnswersLabel = document.createElement('label');
    multipleAnswersLabel.classList.add('form-label');
    multipleAnswersLabel.textContent = 'Multiple answers:';
    questionTitleContainer.appendChild(multipleAnswersLabel);

    // mulitple answers checkbox
    const multipleAnswersInput = document.createElement('input');
    multipleAnswersInput.type = 'checkbox';
    multipleAnswersInput.value = 'false';
    multipleAnswersInput.name = `Questions[${questions.length}].HasMultipleAnswers`;
    multipleAnswersInput.classList.add('form-check-input', 'ms-2');
    multipleAnswersInput.addEventListener('change', (event) => changeQuestionAnswersType(event, question));
    questionTitleContainer.appendChild(multipleAnswersInput);

    // question textarea
    const questionTextarea = document.createElement('textarea');
    questionTextarea.classList.add('form-control', 'mb-3', 'question-textarea');
    questionTextarea.placeholder = 'Question goes here';
    questionTextarea.name = `Questions[${questions.length}].Question`;
    questionContainer.appendChild(questionTextarea);

    // answers section
    const answersSection = document.createElement('section');
    questionContainer.appendChild(answersSection);

    const question = {
        element: questionContainer,
        answersSection,
        answers: 0,
        index: questions.length,
        hasMultipleAnswers: false
    };

    // add new answer button
    const newAnswerButton = document.createElement('button');
    newAnswerButton.textContent = 'Add answer';
    newAnswerButton.classList.add('btn', 'btn-primary');
    newAnswerButton.type = 'button';
    newAnswerButton.addEventListener('click', () => addAnswerField(question));
    questionContainer.appendChild(newAnswerButton);

    // delete question button
    const deleteQuestionButton = document.createElement('button');
    deleteQuestionButton.textContent = 'Delete question';
    deleteQuestionButton.classList.add('btn', 'btn-danger', 'ms-2');
    deleteQuestionButton.type = 'button';
    deleteQuestionButton.addEventListener('click', (event) => deleteQuestion(event, question));
    questionContainer.appendChild(deleteQuestionButton);

    const horizontalLine = document.createElement('hr');
    questionContainer.appendChild(horizontalLine);

    questions.push(question);
    questionsSection.appendChild(questionContainer);
    return question;
}

const addAnswerField = (question) => {
    const answerContainer = document.createElement('div');
    answerContainer.classList.add('mb-3');

    const answerButton = document.createElement('input');
    answerButton.classList.add('form-check-input', 'align-baseline', 'me-2');
    answerButton.type = question.hasMultipleAnswers ? 'checkbox' : 'radio';
    answerButton.name = `answer-${question.index}`;
    answerContainer.appendChild(answerButton);

    const answerInputField = document.createElement('input');
    answerInputField.classList.add('form-control', 'd-inline-block', 'w-75');
    answerInputField.placeholder = 'Answer';
    answerInputField.name = `Questions[${question.index}].Answers[${question.answers}].Content`;
    answerContainer.appendChild(answerInputField);

    const deleteButton = document.createElement('button');
    deleteButton.type = 'button';
    deleteButton.addEventListener('click', (event) => deleteAnswer(event));
    deleteButton.classList.add('btn', 'btn-danger', 'ms-2');
    deleteButton.style.marginTop = '-5px';
    deleteButton.innerHTML = '<i class="fa fa-trash-alt"></i>';
    answerContainer.appendChild(deleteButton);

    question.answers++;
    question.answersSection.appendChild(answerContainer);
}

const createAssignment = () => {
    let questionIndex = 0;
    for (const question of questions) {
        const questionTextarea = question.element.querySelector('#questionsSection > div > textarea');
        questionTextarea.name = `Questions[${questionIndex}].Question`;

        const multipleAnswersInput = question.element.querySelector('#questionsSection > div > div > input');
        multipleAnswersInput.name = `Questions[${questionIndex}].HasMultipleAnswers`;
        multipleAnswersInput.value = multipleAnswersInput.checked;

        let answerIndex = 0;
        const answers = Array.from(question.answersSection.children);
        for (const answer of answers) {
            const valueButton = answer.children[0];
            valueButton.name = `Questions[${questionIndex}].Answers[${answerIndex}].IsCorrect`;
            valueButton.value = valueButton.checked;

            const answerInput = answer.children[1];
            answerInput.name = `Questions[${questionIndex}].Answers[${answerIndex}].Content`;
            answerIndex++;
        }

        questionIndex++;
    }
}

const deleteAnswer = (event) => {
    const answerContainer = event.currentTarget.parentNode;
    answerContainer.remove();
}

const changeQuestionAnswersType = (event, question) => {
    const checkedState = event.target.checked;
    const answersSection = event.currentTarget.parentNode.nextElementSibling.nextElementSibling;
    const answerElements = Array.from(answersSection.children);

    const newButtonType = checkedState ? 'checkbox' : 'radio';
    for (const answer of answerElements) {
        const answerButton = answer.children[0];

        if (newButtonType === 'radio') {
            answerButton.checked = false;
        }

        answerButton.type = newButtonType;
    }

    question.hasMultipleAnswers = checkedState;
}

const deleteQuestion = (event, question) => {
    const deleteConfirm = confirm('Are you sure you want to delete this question ?');
    if (deleteConfirm === false) {
        return;
    }

    const questionIndex = questions.indexOf(question);
    questions.splice(questionIndex, 1);
    question.element.remove();

    let currentQuestionIndex = 1;
    for (const question of questions) {
        const questionLabel = question.element.children[0];
        questionLabel.textContent = `#${currentQuestionIndex} Question`;
        currentQuestionIndex++;
    }
}

window.onload = () => {
    const generatedQuestions = Array.from(questionsSection.children);
    generatedQuestions.forEach((questionElement, index) => {
        const answersSection = questionElement.getElementsByTagName('section')[0];

        const question = {
            element: questionElement,
            answersSection,
            answers: answersSection.children.length,
            index
        };

        const addAnswerButton = questionElement.getElementsByClassName('add-answer')[0];
        addAnswerButton.addEventListener('click', () => addAnswerField(question));

        questions.push(question);
    });
}